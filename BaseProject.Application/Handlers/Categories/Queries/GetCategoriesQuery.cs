using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Handlers.Categories.Queries;

public class GetCategoriesQuery: IRequest<Result>
{
    
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result>
    {
        private readonly IMapper _mapper;
        private readonly IEntityGeneralRepository _repository;

        public GetCategoriesQueryHandler(IMapper mapper, IEntityGeneralRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repository
                .Query<Category>()
                .Where(_=>_.IsDeleted == false)
                .ToListAsync();

            var result = _mapper.Map<List<CategoryDto>>(categories);
            
            return await Result<List<CategoryDto>>
                .SuccessAsync(result);
        }
    }
}