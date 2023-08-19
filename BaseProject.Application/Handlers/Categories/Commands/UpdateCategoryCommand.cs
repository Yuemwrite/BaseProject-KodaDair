using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result>
{
    public long Id { get; set; }

    public string Name { get; set; }

    public byte[] RowVersion { get; set; }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateCategoryCommandHandler(IEntityRepository<Category> categoryRepository, ICurrentUser currentUser)
        {
            _categoryRepository = categoryRepository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await
                _categoryRepository
                    .Query()
                    .FirstOrDefaultAsync(_ => _.Id == request.Id, cancellationToken: cancellationToken);

            if (category is null)
            {
                return await Result<object>
                    .FailAsync(new List<string>()
                    {
                        "Kategori bulunamadı."
                    });
            }

            category.Name = request.Name;
            category.RowVersion = request.RowVersion;
            category.LastModificationTime = DateTime.UtcNow;
            category.LastModifierUserId = _currentUser.GetUserId();

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                RowVersion = category.RowVersion
            };

            return await Result<CategoryDto>
                .SuccessAsync(response);
        }
    }
}