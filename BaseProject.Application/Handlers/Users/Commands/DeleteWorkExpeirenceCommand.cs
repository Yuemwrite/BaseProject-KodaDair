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

public class DeleteWorkExperienceCommand : IRequest<Result>
{
    public long Id { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public class DeleteWorkExperienceCommandHandler : IRequestHandler<DeleteWorkExperienceCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEntityGeneralRepository _repository;
        private readonly IMapper _mapper;

        public DeleteWorkExperienceCommandHandler(ICurrentUser currentUser, IEntityGeneralRepository repository, IMapper mapper)
        {
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteWorkExperienceCommand request, CancellationToken cancellationToken)
        {
            var deletedExperience = await
                _repository
                    .Query<WorkExpeirence>()
                    .FirstOrDefaultAsync(_ => _.Id == request.Id && _.UserId == _currentUser.GetUserId());

            if (deletedExperience is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.ExperienceNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.ExperienceNotFound.GetCustomDisplayName() }
                });
            }

            deletedExperience.IsDeleted = true;
            deletedExperience.DeleterUserId = _currentUser.GetUserId();
            deletedExperience.DeletionTime = DateTime.UtcNow;

            _repository.Update(deletedExperience);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ExperienceDto>(deletedExperience);
            
            return await Result<ExperienceDto>
                .SuccessAsync(result);


        }
    }
}