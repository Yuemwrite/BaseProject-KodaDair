using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Replies.Commands;

public class CreateReplyCommand : IRequest<Result>
{
    public Guid CommentId { get; set; }
    
    public string Content { get; set; }

    public class CreateReplyCommandHandler : IRequestHandler<CreateReplyCommand, Result>
    {
        private readonly IEntityRepository<Comment> _commentRepository;
        private readonly IEntityRepository<Reply> _replyRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public CreateReplyCommandHandler(IEntityRepository<Comment> commentRepository,
            IEntityRepository<Reply> replyRepository, ICurrentUser currentUser, IEntityRepository<User> userRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _replyRepository = replyRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
        {
            if (!_commentRepository.Query().Any(_=>_.Id == request.CommentId))
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.CommentNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.CommentNotFound.GetCustomDisplayName() }
                });
            }

            var reply = new Reply()
            {
                UserId = _currentUser.GetUserId(),
                CommentId = request.CommentId,
                Content = request.Content,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _currentUser.GetUserId(),
                IsDeleted = false
            };

            _replyRepository.Add(reply);
            await _replyRepository.SaveChangesAsync();

            var result = _mapper.Map<ReplyDto>(reply);
            
            return await Result<ReplyDto>.SuccessAsync(result);

        }
    }
}