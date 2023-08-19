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

namespace BaseProject.Application.Handlers.SubCategories;

public class CreateSubCategoryCommand : IRequest<Result>
{
    public long CategoryId { get; set; }

    public string Name { get; set; }

    public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommand, Result>
    {
        private readonly IEntityRepository<SubCategory> _subCategoryRepository;
        private readonly IEntityRepository<Category> _categoryRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public CreateSubCategoryCommandHandler(IEntityRepository<SubCategory> subCategoryRepository,
            ICurrentUser currentUser, IEntityRepository<Category> categoryRepository, IMapper mapper)
        {
            _subCategoryRepository = subCategoryRepository;
            _currentUser = currentUser;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Query()
                .Where(_ => _.Id == request.CategoryId && _.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            if (category is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.CategoryNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.CategoryNotFound.GetCustomDisplayName() }
                });
            }
            
            var subCategory = new SubCategory()
            {
                CategoryId = request.CategoryId,
                Name = request.Name
            };

            _subCategoryRepository.Add(subCategory);
            await _subCategoryRepository.SaveChangesAsync();

            var result = _mapper.Map<SubCategoryDto>(subCategory);
            
            return await Result<SubCategoryDto>
                .SuccessAsync(result);
        }
    }
}