using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Utilities.Encryption;
using BaseProject.Application.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateForgotPasswordTransactionApprovalCommand : IRequest<Result>
{
    public long OneTimePasswordTransactionId { get; set; }

    public Guid UserId { get; set; }

    public string OneTimePassword { get; set; }
    
    public string NewPassword { get; set; }
    

    public class
        CreateForgotPasswordTransactionApprovalCommandHandler : IRequestHandler<
            CreateForgotPasswordTransactionApprovalCommand, Result>
    {
        private readonly ITwoFactorAuthenticationRepository _twoFactorAuthenticationRepository;
        private readonly IUserRepository _userRepository;

        public CreateForgotPasswordTransactionApprovalCommandHandler(
            ITwoFactorAuthenticationRepository twoFactorAuthenticationRepository, IUserRepository userRepository)
        {
            _twoFactorAuthenticationRepository = twoFactorAuthenticationRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(CreateForgotPasswordTransactionApprovalCommand request,
            CancellationToken cancellationToken)
        {
            var twoFactorAuthentication = await
                _twoFactorAuthenticationRepository
                    .Query()
                    .FirstOrDefaultAsync(
                        _ => _.UserId == request.UserId && _.Id == request.OneTimePasswordTransactionId);

            if (twoFactorAuthentication is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.InvalidOneTimePassword).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.InvalidOneTimePassword.GetCustomDisplayName() }
                });
            }

            var oneTimePasswordControl = await _twoFactorAuthenticationRepository.VerifyOtp(request.UserId,
                twoFactorAuthentication.OneTimePasswordType, request.OneTimePassword);

            if (!oneTimePasswordControl)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.InvalidOneTimePassword).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.InvalidOneTimePassword.GetCustomDisplayName() }
                });
            }

            var user = await _userRepository.Query().FirstOrDefaultAsync(_ => _.Id == request.UserId, cancellationToken: cancellationToken);

            if (user is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UserNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UserNotFound.GetCustomDisplayName() }
                });
            }

            user.Password = PasswordToolkit.EnhancedHashPassword(request.NewPassword);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            
            return await Result<object>
                .SuccessAsync("OK.");

        }
    }
}