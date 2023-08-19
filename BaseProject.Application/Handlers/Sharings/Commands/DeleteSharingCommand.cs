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

public class DeleteSharingCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public class DeleteSharingCommandHandler : IRequestHandler<DeleteSharingCommand, Result>
    {
        private readonly IEntityRepository<Sharing> _sharingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public DeleteSharingCommandHandler(IEntityRepository<Sharing> sharingRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _sharingRepository = sharingRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteSharingCommand request, CancellationToken cancellationToken)
        {
            var sharing = await _sharingRepository
                .Query()
                .FirstOrDefaultAsync(_ => _.Id == request.Id && _.IsDeleted == false && _.UserId == _currentUser.GetUserId(), cancellationToken: cancellationToken);

            if (sharing is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.SharingNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.SharingNotFound.GetCustomDisplayName() }
                });
            }

            sharing.RowVersion = request.RowVersion;
            sharing.IsDeleted = false;
            sharing.LastModificationTime = DateTime.UtcNow;
            sharing.LastModifierUserId = _currentUser.GetUserId();

            var result = _mapper.Map<SharingDto>(sharing);

            _sharingRepository.Update(sharing);
            await _sharingRepository.SaveChangesAsync(cancellationToken);
            
            return await Result<SharingDto>
                .SuccessAsync(result);

        }
    }
}