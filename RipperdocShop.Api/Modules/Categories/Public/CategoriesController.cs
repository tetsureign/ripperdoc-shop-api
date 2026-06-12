using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Categories.Queries;

namespace RipperdocShop.Api.Modules.Categories.Public;

[Route("api/categories")]
[ApiController]
public class CategoriesController(ListCategoriesQuery listCategories, GetCategoryBySlugQuery getCategoryBySlug)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var response = await listCategories.ExecuteAsync(false, page, pageSize);
        return Ok(response);
    }
    
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var category = await getCategoryBySlug.ExecuteAsync(slug);
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
