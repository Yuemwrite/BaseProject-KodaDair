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

public class UpdateSharingFixedCommand : IRequest<Result>
{
    public Guid SharingId { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public class UpdateSharingFixedCommandHandler : IRequestHandler<UpdateSharingFixedCommand, Result>
    {
        private readonly IEntityGeneralRepository _generalRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public UpdateSharingFixedCommandHandler(IEntityGeneralRepository generalRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _generalRepository = generalRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateSharingFixedCommand request, CancellationToken cancellationToken)
        {
            var result = new SharingDto();
            
            var sharing = await _generalRepository
                .Query<Sharing>()
                .FirstOrDefaultAsync(_ => _.UserId == _currentUser.GetUserId() && _.Id == request.SharingId && _.IsDeleted == false);

            if (sharing is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.SharingNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.SharingNotFound.GetCustomDisplayName() }
                });
            }

            if (_generalRepository.Query<Sharing>().Any(_=>_.IsFixed))
            {
                var fixedSharing = await _generalRepository
                    .Query<Sharing>()
                    .Where(_ => _.IsFixed && _.Id != request.SharingId)
                    .FirstOrDefaultAsync();

                if (fixedSharing is null)
                {
                    throw new Exception();
                }

                fixedSharing.IsFixed = false;
                fixedSharing.RowVersion = request.RowVersion;

                result = _mapper.Map<SharingDto>(fixedSharing);

                _generalRepository.Update(fixedSharing);
                await _generalRepository.SaveChangesAsync();
            }

            sharing.IsFixed = !sharing.IsFixed;
            sharing.RowVersion = request.RowVersion;

            result = _mapper.Map<SharingDto>(sharing);

            _generalRepository.Update(sharing);
            await _generalRepository.SaveChangesAsync();

            return await Result<SharingDto>
                .SuccessAsync(result);
        }
    }
}