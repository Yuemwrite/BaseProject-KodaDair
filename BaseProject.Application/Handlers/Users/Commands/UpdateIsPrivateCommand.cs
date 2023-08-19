using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class UpdateIsPrivateCommand : IRequest<Result>
{
    
    public class UpdateIsPrivateCommandHandler : IRequestHandler<UpdateIsPrivateCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateIsPrivateCommandHandler(IUserRepository userRepository, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateIsPrivateCommand request, CancellationToken cancellationToken)
        {
            var user = await
                _userRepository
                    .Query()
                    .FirstOrDefaultAsync(_ => _.Id == _currentUser.GetUserId(), cancellationToken: cancellationToken);

            if (user is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UserNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UserNotFound.GetCustomDisplayName() }
                });
            }


            user.IsPrivate = !user.IsPrivate;
            
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);
            
            return await Result<object>
                .SuccessAsync();
        }
    }
}