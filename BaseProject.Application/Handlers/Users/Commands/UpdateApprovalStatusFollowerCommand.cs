using System.Collections;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Base.Auditing;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Users.Commands;

public class UpdateApprovalStatusFollowerCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; }

    public byte[] RowVersion { get; set; }

    public class
        UpdateApprovalStatusFollowerCommandHandler : IRequestHandler<UpdateApprovalStatusFollowerCommand, Result>
    {
        private readonly IEntityGeneralRepository _repository;
        private readonly ICurrentUser _currentUser;

        public UpdateApprovalStatusFollowerCommandHandler(IEntityGeneralRepository repository, ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateApprovalStatusFollowerCommand request,
            CancellationToken cancellationToken)
        {
            var pendingFollow = await
                _repository
                    .Query<Follower>()
                    .FirstOrDefaultAsync(_ => _.Id == request.Id, cancellationToken: cancellationToken);

            if (pendingFollow is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.UserNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.UserNotFound.GetCustomDisplayName() }
                });
            }

            pendingFollow.ApprovalStatus = request.ApprovalStatus switch
            {
                ApprovalStatus.Approved => ApprovalStatus.Approved,
                ApprovalStatus.Rejected => ApprovalStatus.Rejected,
                _ => throw new Exception()
            };

            pendingFollow.LastModifierUserId = _currentUser.GetUserId();
            pendingFollow.LastModificationTime = DateTime.UtcNow;
            pendingFollow.RowVersion = request.RowVersion;
            pendingFollow.IsActive = request.ApprovalStatus == ApprovalStatus.Approved;

            _repository.Update(pendingFollow);

            
            await _repository.SaveChangesAsync();


            var response = new FollowerDto()
            {
                Id = pendingFollow.Id,
                UserId = pendingFollow.FollowedUserId,
                UserName =
                    _repository.Query<User>().FirstOrDefault(_ => _.Id == pendingFollow.FollowedUserId)!.UserName,
                ApprovalStatus = pendingFollow.ApprovalStatus,
                CreationTime = pendingFollow.CreationTime,
                FollowerUserId = pendingFollow.FollowerUserId,
                FollowerUserName = _repository.Query<User>().FirstOrDefault(_ => _.Id == pendingFollow.FollowerUserId)!
                    .UserName,
                RowVersion = pendingFollow.RowVersion
            };

            return await Result<FollowerDto>
                .SuccessAsync(response);
        }
    }
}