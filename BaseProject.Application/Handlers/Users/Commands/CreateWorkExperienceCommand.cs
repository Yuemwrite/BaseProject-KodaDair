using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateWorkExperienceCommand : IRequest<Result>
{
    public string CompanyName { get; set; }
    
    public string? Remark { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public class CreateWorkExperienceCommandHandler : IRequestHandler<CreateWorkExperienceCommand, Result>
    {
        private readonly IEntityGeneralRepository _repository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public CreateWorkExperienceCommandHandler(IEntityGeneralRepository repository, ICurrentUser currentUser, IMapper mapper)
        {
            _repository = repository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateWorkExperienceCommand request, CancellationToken cancellationToken)
        {
            var experience = new WorkExpeirence()
            {
                UserId = _currentUser.GetUserId(),
                CompanyName = request.CompanyName,
                Remark = request.Remark,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _currentUser.GetUserId()
            };

            _repository.Add(experience);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ExperienceDto>(experience);
            
            return await Result<ExperienceDto>
                .SuccessAsync(result);
        }
    }
}