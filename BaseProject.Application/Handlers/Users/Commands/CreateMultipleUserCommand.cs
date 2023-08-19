using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateMultipleUserCommand : IRequest<Result>
{
    public int Count { get; set; }
    
    public class CreateMultipleUserCommandHandler : IRequestHandler<CreateMultipleUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public CreateMultipleUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(CreateMultipleUserCommand request, CancellationToken cancellationToken)
        {
            for (int i = 0; i < request.Count; i++)
            {
                User newUser = new User()
                {
                    UserName = "Test",
                    Password = "12345",
                    ExpirationTime = DateTime.Now
                };

                _userRepository.Add(newUser);
                await _userRepository.SaveChangesAsync();
            }
            
            return await Result<object>
                .SuccessAsync("OK");
        }
    }
}