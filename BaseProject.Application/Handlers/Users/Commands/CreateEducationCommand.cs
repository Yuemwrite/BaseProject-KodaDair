using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;

namespace BaseProject.Application.Handlers.Users.Commands;

public class CreateEducationCommand : IRequest<Result>
{
    public EducationLevel EducationLevel { get; set; }
    
    public string SchoolName { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }


    public class CreateEducationCommandHandler : IRequestHandler<CreateEducationCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEntityGeneralRepository _repository;
        private readonly IMapper _mapper;
        
        public CreateEducationCommandHandler(ICurrentUser currentUser, IEntityGeneralRepository repository, IMapper mapper)
        {
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateEducationCommand request, CancellationToken cancellationToken)
        {
            var newEducation = new Education()
            {
                EducationLevel = request.EducationLevel,
                SchoolName = request.SchoolName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                UserId = _currentUser.GetUserId()
            };

            _repository.Add(newEducation);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<EducationDto>(newEducation);

            return await Result<EducationDto>
                .SuccessAsync(result);


        }
    }
}