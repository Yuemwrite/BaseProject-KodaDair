using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class UpdateWorkExperienceCommand : IRequest<Result>
{
    public long Id { get; set; }
    
    public string CompanyName { get; set; }
    
    public string? Remark { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public class UpdateWorkExperienceCommandHandler : IRequestHandler<UpdateWorkExperienceCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEntityGeneralRepository _repository;
        private readonly IMapper _mapper;

        public UpdateWorkExperienceCommandHandler(ICurrentUser currentUser, IEntityGeneralRepository repository, IMapper mapper)
        {
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateWorkExperienceCommand request, CancellationToken cancellationToken)
        {
            var updateExperience = await _repository
                .Query<WorkExpeirence>()
                .Where(_ => _.Id == request.Id && _.UserId == _currentUser.GetUserId())
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (updateExperience is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.ExperienceNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.ExperienceNotFound.GetCustomDisplayName() }
                });
            }

            updateExperience.CompanyName = request.CompanyName;
            updateExperience.Remark = request.Remark;
            updateExperience.StartDate = request.StartDate;
            updateExperience.EndDate = request.EndDate;
            updateExperience.RowVersion = request.RowVersion;

            _repository.Update(updateExperience);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ExperienceDto>(updateExperience);
            
            return await Result<ExperienceDto>
                .SuccessAsync(result);
        }
    }
}