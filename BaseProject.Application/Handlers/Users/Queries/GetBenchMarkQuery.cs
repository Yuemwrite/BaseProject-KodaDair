using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Wrapper;
using MediatR;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetBenchMarkQuery : IRequest<Result>
{
    public class GetBenchMarkQueryHandler : IRequestHandler<GetBenchMarkQuery, Result>
    {
        private readonly IUserRepository _userRepository;

        public GetBenchMarkQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(GetBenchMarkQuery request, CancellationToken cancellationToken)
        {
            
           return await Result<object>
               .SuccessAsync(  _userRepository.BenchMarkTest());
        }
    }
}