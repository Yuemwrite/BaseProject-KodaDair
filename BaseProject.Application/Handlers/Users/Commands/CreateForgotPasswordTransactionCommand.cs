using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Utilities.Encryption;
using BaseProject.Application.Utilities.Toolkit;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateForgotPasswordTransactionCommand : IRequest<Result>
{
    public OneTimePasswordChannel Channel { get; set; }

    public string UserName { get; set; }

    public class
        CreateForgotPasswordTransactionCommandHandler : IRequestHandler<CreateForgotPasswordTransactionCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITwoFactorAuthenticationRepository _twoFactorAuthenticationRepository;
        

        public CreateForgotPasswordTransactionCommandHandler(IUserRepository userRepository,
            ITwoFactorAuthenticationRepository twoFactorAuthenticationRepository)
        {
            _userRepository = userRepository;
            _twoFactorAuthenticationRepository = twoFactorAuthenticationRepository;
        }

        public async Task<Result> Handle(CreateForgotPasswordTransactionCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository
                .Query()
                .FirstOrDefaultAsync(_ => _.UserName == request.UserName);

            if (user is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UserNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UserNotFound.GetCustomDisplayName() }
                });
            }

            var to = request.Channel == OneTimePasswordChannel.Email ? user.Email : user.MobilePhoneNumber;

            var oneTimePasswordTransaction = await _twoFactorAuthenticationRepository.CreateOtp(user.Id,
                OneTimePasswordType.ForgotPassword, to,
                request.Channel);

            return await Result<OneTimePasswordDto>
                .SuccessAsync(new OneTimePasswordDto()
                {
                    Success = true,
                    OneTimePasswordId = oneTimePasswordTransaction.OneTimePasswordId,
                    UserId = user.Id
                });
        }
    }
}