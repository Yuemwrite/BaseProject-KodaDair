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

public class DeleteEducationCommand : IRequest<Result>
{
    public long Id { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public class DeleteEducationCommandHandler : IRequestHandler<DeleteEducationCommand, Result>
    {

        private readonly IEntityGeneralRepository _repository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public DeleteEducationCommandHandler(IEntityGeneralRepository repository, ICurrentUser currentUser, IMapper mapper)
        {
            _repository = repository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
        {
            var education = await
                _repository
                    .Query<Education>()
                    .FirstOrDefaultAsync(_ => _.Id == request.Id && _.UserId == _currentUser.GetUserId());

            if (education is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.EducationNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.EducationNotFound.GetCustomDisplayName() }
                });
            }

            education.RowVersion = request.RowVersion;
            education.IsDeleted = true;
            education.DeleterUserId = _currentUser.GetUserId();
            education.DeletionTime = DateTime.UtcNow;
            
            _repository.Update(education);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<EducationDto>(education);
            
            return await Result<EducationDto>
                .SuccessAsync(result);

        }
    }
}