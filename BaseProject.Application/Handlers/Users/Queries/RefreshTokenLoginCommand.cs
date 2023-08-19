using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Identity.Tokens;
using BaseProject.Application.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BaseProject.Application.Handlers.Users.Queries;

public class RefreshTokenLoginCommand : IRequest<Result>
{
    public string RefreshToken { get; set; }

    public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public RefreshTokenLoginCommandHandler(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(RefreshTokenLoginCommand request, CancellationToken cancellationToken)
        {
            var userRefreshToken = await _userRepository.Query().Where(_ => _.RefreshToken == request.RefreshToken)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (userRefreshToken == null || userRefreshToken!.RefreshTokenTime <= DateTime.Now)
                throw new Exception("Refresh token bulunamadı.");
            var jwtConfig = _configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>(); 
            var token = _tokenService.CreateToken(userRefreshToken, jwtConfig);

            userRefreshToken.RefreshToken = token.RefreshToken;
            userRefreshToken.RefreshTokenTime = token.ExpirationTime.AddSeconds(20);

            _userRepository.Update(userRefreshToken);
            await _userRepository.SaveChangesAsync();

            return await Result<object>
                .SuccessAsync(token);
        }
    }
}