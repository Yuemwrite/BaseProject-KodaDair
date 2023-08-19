using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetWorkExperienceQuery : IRequest<Result>
{
    public long Id { get; set; }
    
    public class GetWorkExperienceQueryHandler : IRequestHandler<GetWorkExperienceQuery, Result>
    {
        private readonly IEntityGeneralRepository _repository;

        public GetWorkExperienceQueryHandler(IEntityGeneralRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(GetWorkExperienceQuery request, CancellationToken cancellationToken)
        {
            var experience = await
                _repository
                    .Query<WorkExpeirence>()
                    .Where(_ => _.Id == request.Id)
                    .FirstOrDefaultAsync();
            
            return await Result<object>
                .SuccessAsync(experience);
        }
    }
}