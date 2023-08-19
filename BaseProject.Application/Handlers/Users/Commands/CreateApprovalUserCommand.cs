using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateApprovalUserCommand : IRequest<Result>
{
    public Guid TransactionUserId { get; set; }

    public string OneTimePassword { get; set; }

    public class CreateApprovalUserCommandHandler : IRequestHandler<CreateApprovalUserCommand, Result>
    {
        private readonly ITwoFactorAuthenticationRepository _twoFactorAuthenticationRepository;
        private readonly IEntityRepository<TransactionUser> _transactionUserRepository;
        private readonly IEntityRepository<User> _userRepository;

        public CreateApprovalUserCommandHandler(ITwoFactorAuthenticationRepository twoFactorAuthenticationRepository,
            IEntityRepository<TransactionUser> transactionUserRepository,
            IEntityRepository<User> userRepository)
        {
            _twoFactorAuthenticationRepository = twoFactorAuthenticationRepository;
            _transactionUserRepository = transactionUserRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(CreateApprovalUserCommand request, CancellationToken cancellationToken)
        {
            var transactionUser = await _transactionUserRepository
                .Query()
                .FirstAsync(_ => _.Id == request.TransactionUserId);

            var verify = await _twoFactorAuthenticationRepository.VerifyOtp(request.TransactionUserId,
                OneTimePasswordType.Register, request.OneTimePassword);

            if (!verify)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.OneTimePasswordNotBeVerified).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.OneTimePasswordNotBeVerified.GetCustomDisplayName() }
                });
            }

            var newUser = new User()
            {
                Id = transactionUser.Id,
                UserName = transactionUser.UserName,
                Email = transactionUser.Email,
                MobilePhoneNumber = transactionUser.MobilePhoneNumber,
                Password = transactionUser.Password,
                ExpirationTime = DateTime.UtcNow.AddYears(3),
                IsPrivate = false
            };

            _userRepository.Add(newUser);
            await _userRepository.SaveChangesAsync();

            return await Result<User>
                .SuccessAsync(newUser);
        }
    }
}