using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;

namespace BaseProject.Application.Handlers.Categories.Commands;

public class CreateCategoryCommand : IRequest<Result>
{
    public string Name { get; set; }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result>
    {
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly ICurrentUser _currentUser;

        public CreateCategoryCommandHandler(IEntityRepository<Category> categoryRepository, ICurrentUser currentUser)
        {
            _categoryRepository = categoryRepository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category()
            {   
                Name = request.Name,
                CreatorUserId = _currentUser.GetUserId()
            };

            _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync();
            
            return await Result<CategoryDto>
                .SuccessAsync(new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name,
                    RowVersion = category.RowVersion
                });
        }
    }
}