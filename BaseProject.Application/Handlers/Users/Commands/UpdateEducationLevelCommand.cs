using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class UpdateEducationCommand : IRequest<Result>
{
    public long Id { get; set; }

    public EducationLevel EducationLevel { get; set; }

    public string SchoolName { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public byte[] RowVersion { get; set; }

    public class UpdateEducationLevelCommandHandler : IRequestHandler<UpdateEducationCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEntityGeneralRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEducationLevelCommandHandler(ICurrentUser currentUser, IEntityGeneralRepository repository, IMapper mapper)
        {
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateEducationCommand request, CancellationToken cancellationToken)
        {
            var education = await _repository.Query<Education>()
                .FirstOrDefaultAsync(_ => _.Id == request.Id && _.UserId == _currentUser.GetUserId());

            if (education is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.EducationNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.EducationNotFound.GetCustomDisplayName() }
                });
            }

            education.EducationLevel = request.EducationLevel;
            education.SchoolName = request.SchoolName;
            education.StartDate = request.StartDate;
            education.EndDate = request.EndDate;
            education.RowVersion = request.RowVersion;

            _repository.Update(education);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<EducationDto>(education);


            return await Result<EducationDto>
                .SuccessAsync(result);
        }
    }
}