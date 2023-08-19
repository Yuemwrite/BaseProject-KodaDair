using AutoMapper;
using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Likes.Commands;

public class CreateOrUpdateLikeCommand : IRequest<Result>
{
    public long? Id { get; set; }

    public Guid? ContentId { get; set; }

    public byte[]? RowVersion { get; set; }

    public class UpdateLikeCommandHandler : IRequestHandler<CreateOrUpdateLikeCommand, Result>
    {
        private readonly IEntityGeneralRepository _generalRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IEntityRepository<Like> _likeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateLikeCommandHandler(IEntityGeneralRepository generalRepository, ICurrentUser currentUser,
            IEntityRepository<Like> likeRepository, IUserRepository userRepository, IMapper mapper)
        {
            _generalRepository = generalRepository;
            _currentUser = currentUser;
            _likeRepository = likeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateOrUpdateLikeCommand request, CancellationToken cancellationToken)
        {
            var result = new LikeDto();
            
            var likeInfo = await
                _likeRepository
                    .Query()
                    .FirstOrDefaultAsync(_ => _.Id == request.Id && _.UserId == _currentUser.GetUserId());

            if (likeInfo is not null)
            {
                likeInfo.IsActive = !likeInfo.IsActive;
                likeInfo.RowVersion = request.RowVersion!;

                result = _mapper.Map<LikeDto>(likeInfo);
                
                _likeRepository.Update(likeInfo);
                await _likeRepository.SaveChangesAsync();
                
                return await Result<LikeDto>
                    .SuccessAsync(result);
            }

            var newLike = new Like();
            newLike.UserId = _currentUser.GetUserId();
            newLike.CreationTime = DateTime.UtcNow;
            newLike.CreatorUserId = _currentUser.GetUserId();
            newLike.UserId = _currentUser.GetUserId();
            newLike.IsActive = true;
            
            if (_generalRepository.Query<Sharing>().Any(_ => _.Id == request.ContentId))
            {
                newLike.SharingId = request.ContentId;
                newLike.CommentId = null;
                newLike.ReplyId = null;
            }

            if (_generalRepository.Query<Comment>().Any(_ => _.Id == request.ContentId))
            {
                newLike.CommentId = request.ContentId;
                newLike.SharingId = null;
                newLike.ReplyId = null;
            }
            
            if (_generalRepository.Query<Reply>().Any(_ => _.Id == request.ContentId))
            {
                newLike.ReplyId = request.ContentId;
                newLike.SharingId = null;
                newLike.CommentId = null;
            }

            _likeRepository.Add(newLike);
            await _likeRepository.SaveChangesAsync();

            result = _mapper.Map<LikeDto>(newLike);
            
            return await Result<LikeDto>
                .SuccessAsync(result);
        }
    }
}