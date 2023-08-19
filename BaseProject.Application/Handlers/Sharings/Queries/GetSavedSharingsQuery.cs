using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Handlers.PageBase;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Sharings.Queries;

public class GetSavedSharingsQuery : PagedSearchQuery<Result>
{
    public class GetSavedSharingsQueryHandler : PagedSearchQueryHandler<GetSavedSharingsQuery, Result>
    {
        private readonly IEntityGeneralRepository _generalRepository;
        private readonly ICurrentUser _currentUser;

        public GetSavedSharingsQueryHandler(IEntityGeneralRepository generalRepository, ICurrentUser currentUser)
        {
            _generalRepository = generalRepository;
            _currentUser = currentUser;
        }

        public override async Task<Result> Handle(GetSavedSharingsQuery request, CancellationToken cancellationToken)
        {
            var savedPost = _generalRepository.Query<SavedPost>();

            var savedPostQuery = savedPost
                .Include(_ => _.Sharing)
                .Where(_=>_.IsActive && _.UserId == _currentUser.GetUserId())
                .Select(_ => new SavedSharingDto()
                {
                    Id = _.Id,
                    UserId = _.UserId,
                    SharingId = _.Sharing.Id,
                    SharingTitle = _.Sharing.Title,
                    IsActive = _.IsActive,
                    RowVersion = _.RowVersion
                }).AsNoTracking();
            
            var result = await _generalRepository.GetPagedResult(savedPostQuery,
                pageSize: request.PageSize,
                pageIndex: request.Page,
                ordering: shr => shr.OrderByDescending(_ => _.Id),
                cancellationToken: cancellationToken);
            
            return HandleResult(result);
        }
    }
}