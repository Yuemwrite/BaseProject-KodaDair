using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.MongoDb;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Comments.Commands;

public class CreateCommentCommand : IRequest<Result>
{
    public Guid SharingId { get; set; }

    public string Comment { get; set; }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result>
    {
        private readonly IEntityRepository<Sharing> _sharingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IEntityRepository<Comment> _commentRepository;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IMongoDbService _mongoDbService;

        public CreateCommentCommandHandler(
            ICurrentUser currentUser,
            IEntityRepository<Sharing> sharingRepository, IEntityRepository<Comment> commentRepository, IEntityRepository<User> userRepository, IMapper mapper, IMongoDbService mongoDbService)
        {
            _sharingRepository = sharingRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _mongoDbService = mongoDbService;
            _currentUser = currentUser;
            _sharingRepository = sharingRepository;
        }

        public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            if (!_sharingRepository.Query().Any(_ => _.Id == request.SharingId))
            {
                var user = await _userRepository.Query().FirstAsync(_ => _.Id == _currentUser.GetUserId());
                
                var errorLog = new Domain.Concrete.ErrorLog()
                {
                    Id = ObjectId.GenerateNewId(),
                    CreationTime = DateTime.UtcNow,
                    UserName = user.UserName,
                    ErrorType = ApiException.ExceptionMessages.SharingNotFound.ToString(),
                    ErrorDescription = ApiException.ExceptionMessages.SharingNotFound.GetCustomDisplayName()
                };

                await _mongoDbService.Create(errorLog, "ErrorLog");
                
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.SharingNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.SharingNotFound.GetCustomDisplayName() }
                });
            }
            
            var comment = new Comment()
            {
                SharingId = request.SharingId,
                UserId = _currentUser.GetUserId(),
                Content = request.Comment,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _currentUser.GetUserId(),
            };

            _commentRepository.Add(comment);
            await _commentRepository.SaveChangesAsync();

            var result = _mapper.Map<CommentDto>(comment);

            return await Result<CommentDto>
                .SuccessAsync(result);
        }
    }
}