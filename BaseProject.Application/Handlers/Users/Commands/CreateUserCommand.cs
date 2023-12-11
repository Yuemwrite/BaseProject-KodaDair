using System.Text.Json;
using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.MongoDb;
using BaseProject.Application.Utilities.Encryption;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateUserCommand : IRequest<Result>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
    
    public string MobilePhoneNumber { get; set; }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<TransactionUser> _transactionUserRepository;
        private readonly ITwoFactorAuthenticationRepository _twoFactorAuthenticationRepository;
        private readonly IMongoDbService _mongoDbService;

        public CreateUserCommandHandler(IEntityRepository<User> userRepository,
            IEntityRepository<TransactionUser> transactionUserRepository,
            ITwoFactorAuthenticationRepository twoFactorAuthenticationRepository, IMongoDbService mongoDbService)
        {
            _userRepository = userRepository;
            _transactionUserRepository = transactionUserRepository;
            _twoFactorAuthenticationRepository = twoFactorAuthenticationRepository;
            _mongoDbService = mongoDbService;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (_userRepository.Query().Any(_ => _.UserName == request.UserName))
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.AlreadyExistUserName).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.AlreadyExistUserName.GetCustomDisplayName() }
                });
            }

            var transactionUser = new TransactionUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = PasswordToolkit.EnhancedHashPassword(request.Password),
                MobilePhoneNumber = request.MobilePhoneNumber
            };

            _transactionUserRepository.Add(transactionUser);
            await _transactionUserRepository.SaveChangesAsync();

            var oneTimePassword = await _twoFactorAuthenticationRepository.CreateOtp(transactionUser.Id,
                OneTimePasswordType.Register,
                transactionUser.Email, OneTimePasswordChannel.Email);


            // var report = new Report()
            // {
            //     Id = ObjectId.GenerateNewId(),
            //     Name = JsonSerializer.Serialize(request)
            // };
            //
            // await _mongoDbService.Create(report, "Report");
            
            return await Result<object>
                .SuccessAsync(new
                {
                    transactionUser.Id,
                    transactionUser.Email,
                    oneTimePassword
                });
        }
    }
}