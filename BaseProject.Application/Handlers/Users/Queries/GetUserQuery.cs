using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using MediatR;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetUserQuery : IRequest<Result>
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result>
    {
        private readonly ICurrentUser _currentUser;

        public GetUserQueryHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await Result<object>
                .SuccessAsync(_currentUser.GetUserId());
        }
    }
}