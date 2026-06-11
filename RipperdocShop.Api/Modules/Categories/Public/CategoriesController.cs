using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Categories.Customer;

namespace RipperdocShop.Api.Modules.Categories.Public;

[Route("api/categories")]
[ApiController]
public class CategoriesController(ICustomerCategoryService categoryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var response = await categoryService.GetAllAsync(false, page, pageSize);
        return Ok(response);
    }
    
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var category = await categoryService.GetBySlugAsync(slug);
        return category == null
            ? NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Category not found",
                Detail = $"Category with slug {slug} does not exist"
            })
            : Ok(category);
    }
}
