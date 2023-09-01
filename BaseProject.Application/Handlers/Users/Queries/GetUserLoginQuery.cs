using Azure;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Identity.Tokens;
using BaseProject.Application.MongoDb;
using BaseProject.Application.Utilities.Encryption;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetUserLoginQuery : IRequest<Result>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public class GetUserLoginQueryHandler : IRequestHandler<GetUserLoginQuery, Result>
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetUserLoginQueryHandler> _logger;
        private readonly IMongoDbService _mongoDbService;

        public GetUserLoginQueryHandler(IEntityRepository<User> userRepository, ITokenService tokenService,
            IConfiguration configuration, ILogger<GetUserLoginQueryHandler> logger, IMongoDbService mongoDbService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _configuration = configuration;
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        public async Task<Result> Handle(GetUserLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Query().Where(_ => _.UserName == request.UserName)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            
            if (user is null)
            {
                _logger.LogError("Kullanıcı adı veya şifre yanlış");
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect.GetCustomDisplayName() }
                });
            }

            var validatePassword = PasswordToolkit.EnhancedVerify(request.Password, user.Password);

            if (!validatePassword)
            {
                var errorLog = new Domain.Concrete.ErrorLog()
                {
                    Id = ObjectId.GenerateNewId(),
                    CreationTime = DateTime.UtcNow,
                    UserName = user.UserName,
                    ErrorType = ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect.ToString(),
                    ErrorDescription = ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect.GetCustomDisplayName()
                };

                await _mongoDbService.Create(errorLog, "ErrorLog");
                
                _logger.LogError("Kullanıcı adı veya şifre yanlış");
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UsernameOrPasswordIsIncorrect.GetCustomDisplayName() }
                });
            }

            var jwtConfig = _configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();


            var token = _tokenService.CreateToken(user, jwtConfig);

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenTime = token.ExpirationTime.AddMinutes(5);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation($"{user.UserName} token aldı. Sisteme giriş yaptı.");

            var response = new LoginDto()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Token = token.AccessToken,
                RefreshToken = token.RefreshToken,
                TokenExpirationTime = token.ExpirationTime,
            };
            
            return await Result<LoginDto>
                .SuccessAsync(response);
        }
    }
}