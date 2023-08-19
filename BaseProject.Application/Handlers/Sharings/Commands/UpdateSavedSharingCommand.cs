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

namespace BaseProject.Application.Handlers.Sharings.Commands;

public class UpdateSavedSharingCommand : IRequest<Result>
{
    public Guid SharingId { get; set; }
    
    public class UpdateSavedSharingCommandHandler : IRequestHandler<UpdateSavedSharingCommand, Result>
    {
        private readonly IEntityGeneralRepository _generalRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public UpdateSavedSharingCommandHandler(IEntityGeneralRepository generalRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _generalRepository = generalRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateSavedSharingCommand request, CancellationToken cancellationToken)
        {
            var result = new SavedSharingDto();
            
            if (_generalRepository.Query<SavedPost>().Any(_=>_.UserId == _currentUser.GetUserId() && _.SharingId == request.SharingId))
            {
                var savedInfo = await _generalRepository.Query<SavedPost>().FirstOrDefaultAsync(_ =>
                    _.UserId == _currentUser.GetUserId() && _.SharingId == request.SharingId);

                if (savedInfo is null)
                {
                    return (Result)await Result.FailAsync(new ErrorInfo()
                    {
                        Code = ((int)ApiException.ExceptionMessages.SharingNotFound).ToString(),
                        Message = new List<string>() { ApiException.ExceptionMessages.SharingNotFound.GetCustomDisplayName() }
                    });
                }

                savedInfo.IsActive = !savedInfo.IsActive;

                _generalRepository.Update(savedInfo);
                await _generalRepository.SaveChangesAsync();

                result = _mapper.Map<SavedSharingDto>(savedInfo);
                
                return await Result<SavedSharingDto>
                    .SuccessAsync(result);
            }

            var newSavedPost = new SavedPost()
            {
                UserId = _currentUser.GetUserId(),
                SharingId = request.SharingId,
                IsActive = true,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _currentUser.GetUserId(),
            };

            _generalRepository.Add(newSavedPost);
            await _generalRepository.SaveChangesAsync();

            result = _mapper.Map<SavedSharingDto>(newSavedPost);
            
            
            return await Result<SavedSharingDto>
                .SuccessAsync(result);
        }
    }
}