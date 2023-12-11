using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using Domain.Enum;
using MediatR;

namespace BaseProject.Application.Handlers.Sharings.Commands;

public class CreateSharingCommand : IRequest<Result>
{
    public string Title { get; set; }

    public string Content { get; set; }

    public long CategoryId { get; set; }

    public long SubCategoryId { get; set; }


    public class CreateSharingCommandHandler : IRequestHandler<CreateSharingCommand, Result>
    {
        private readonly IEntityRepository<Sharing> _sharingRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IEntityGeneralRepository _repository;

        public CreateSharingCommandHandler(IEntityRepository<Sharing> sharingRepository, ICurrentUser currentUser, IEntityRepository<Category> categoryRepository, IMapper mapper, IEntityGeneralRepository repository)
        {
            _sharingRepository = sharingRepository;
            _currentUser = currentUser;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result> Handle(CreateSharingCommand request, CancellationToken cancellationToken)
        {
            var isCategory =  _categoryRepository.Query().Any(_ => _.Id == request.CategoryId);

            if (!isCategory)
            {
                throw new UserFriendlyException(ApiException.ExceptionMessages.CategoryNotFound);
            }

            // using var dbTransaction = _sharingRepository.BeginTransaction();
            
            var sharing = new Sharing()
            {
                UserId = _currentUser.GetUserId(),
                SharingType = SharingType.Question,
                Title = request.Title,
                Content = request.Content,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _currentUser.GetUserId(),
                CategoryId = request.CategoryId,
                SubCategoryId = request.SubCategoryId
            };

            _sharingRepository.Add(sharing);
            await _sharingRepository.SaveChangesAsync();

            var comment = new Comment()
            {
                UserId = _currentUser.GetUserId(),
                SharingId = new Guid(),
                Content = "test",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _currentUser.GetUserId()
            };

            _repository.Add(comment);
            await _repository.SaveChangesAsync();
            
           // dbTransaction.Commit();


            var result = _mapper.Map<SharingDto>(sharing);

            return await Result<SharingDto>
                .SuccessAsync(result);
        }
    }
}