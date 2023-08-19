using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.SubCategories.Queries;

public class GetSubCategoriesQuery : IRequest<Result>
{
    public long CategoryId { get; set; }
    
    public class GetSubCategoriesQueryHandler : IRequestHandler<GetSubCategoriesQuery, Result>
    {
        private readonly IEntityGeneralRepository _repository;
        private readonly IMapper _mapper;

        public GetSubCategoriesQueryHandler(IEntityGeneralRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.Query<Category>()
                .Where(_ => _.Id == request.CategoryId && _.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (category is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.CategoryNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.CategoryNotFound.GetCustomDisplayName() }
                });
            }
            

            var subCategories = await
                _repository
                    .Query<SubCategory>()
                    .Where(_ => _.CategoryId == request.CategoryId)
                    .ToListAsync();
            
            var result = _mapper.Map<List<SubCategoryDto>>(subCategories);
            
            return await Result<List<SubCategoryDto>>
                .SuccessAsync(result);
            
        }
    }
}