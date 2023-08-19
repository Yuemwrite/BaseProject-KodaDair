using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Comments.Commands;

public class UpdateCommentCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    
    public string Comment { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result>
    {
        private readonly IEntityRepository<Comment> _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateCommentCommandHandler(IEntityRepository<Comment> commentRepository, ICurrentUser currentUser, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await
                _commentRepository
                    .Query()
                    .FirstOrDefaultAsync(_ => _.UserId == _currentUser.GetUserId() && _.Id == request.Id);

            if (comment is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.CommentNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.CommentNotFound.GetCustomDisplayName() }
                });
            }

            comment.Content = request.Comment;
            comment.RowVersion = request.RowVersion;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync(cancellationToken);
            
            var commentResponse = new CommentDto()
            {
                Id = comment.Id,
                SharingId = comment.SharingId,
                Content = comment.Content,
                UserId = comment.UserId,
                UserName = _userRepository.Query().SingleOrDefault(_ => _.Id == comment.UserId)!.UserName,
                CreationTime = comment.CreationTime,
                RowVersion = comment.RowVersion
            };

            return await Result<CommentDto>
                .SuccessAsync(commentResponse);
        }
    }
}