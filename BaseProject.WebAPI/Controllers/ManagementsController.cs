using BaseProject.Application.Handlers.Categories.Commands;
using BaseProject.Application.Handlers.Categories.Queries;
using BaseProject.Application.Handlers.SubCategories;
using BaseProject.Application.Handlers.SubCategories.Queries;
using BaseProject.WebAPI.Controllers.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class ManagementsController : BaseApiController
{
    /// <summary>Kategori Oluştur</summary>
    /// <response code="200"></response>
    [HttpPost("category/create")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand createCategoryCommand)
    {
        return Created("", await Mediator.Send(createCategoryCommand));
    }
    
    /// <summary>Kategori Listele</summary>
    /// <response code="200"></response>
    [HttpGet("category/list")]
    public async Task<ActionResult> GetCategories()
    {
        return Ok(await Mediator.Send(new GetCategoriesQuery()));
    }
    
    /// <summary>Kategoriye Ait Alt Kategori Oluştur</summary>
    /// <response code="200"></response>
    [HttpPost("subcategory/create")]
    public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryCommand createSubCategoryCommand)
    {
        return Created("", await Mediator.Send(createSubCategoryCommand));
    }
    
    /// <summary>Kategoriye Ait Alt Kategorileri Listele</summary>
    /// <response code="200"></response>
    [HttpGet("subcategory/list")]
    public async Task<ActionResult> GetSubCategories(long categoryId)
    {
        return Ok(await Mediator.Send(new GetSubCategoriesQuery(){CategoryId = categoryId}));
    }
}