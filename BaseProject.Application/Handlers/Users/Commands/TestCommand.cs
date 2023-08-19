using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;

namespace BaseProject.Application.Handlers.Users.Commands;

public class TestCommand : IRequest<Result>
{
    public class TestCommandHandler : IRequestHandler<TestCommand, Result>
    {
        private readonly IEntityRepository<User> _entityRepository;

        public TestCommandHandler(IEntityRepository<User> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<Result> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                UserName = "Esra",
            };

            _entityRepository.Add(user);
            await _entityRepository.SaveChangesAsync();
            
            return await Result<object>
                 .SuccessAsync("OK");
        }
    }
}