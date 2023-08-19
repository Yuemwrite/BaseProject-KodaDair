using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetEducationQuery : IRequest<Result>
{
    public Guid? UserId { get; set; }
    
    public class GetEducationQueryHandler : IRequestHandler<GetEducationQuery, Result>
    {
        private readonly IEntityGeneralRepository _repository;
        private readonly ICurrentUser _currentUser;
        
        
        public GetEducationQueryHandler(IEntityGeneralRepository repository, ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(GetEducationQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId ?? _currentUser.GetUserId();

            var education = await
                _repository
                    .Query<Education>()
                    .Where(_ => _.UserId == userId && _.IsDeleted == false)
                    .ToListAsync();
            
            return await Result<List<Education>>
                .SuccessAsync(education);
            
            
        }
    }
}