using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetWorkExperiencesQuery : IRequest<Result>
{
    public Guid? UserId { get; set; }
    
    public class GetWorkExperiencesQueryHandler : IRequestHandler<GetWorkExperiencesQuery, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEntityGeneralRepository _repository;

        public GetWorkExperiencesQueryHandler(ICurrentUser currentUser, IEntityGeneralRepository repository)
        {
            _currentUser = currentUser;
            _repository = repository;
        }

        public async Task<Result> Handle(GetWorkExperiencesQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId ?? _currentUser.GetUserId();

            var experiences = await
                _repository
                    .Query<WorkExpeirence>()
                    .Where(_ => _.UserId == userId)
                    .ToListAsync();
            
            return await Result<List<WorkExpeirence>>
                .SuccessAsync(experiences);
        }
    }
}