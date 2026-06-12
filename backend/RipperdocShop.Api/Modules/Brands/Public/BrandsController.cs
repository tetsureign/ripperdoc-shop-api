using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Brands.Queries;

namespace RipperdocShop.Api.Modules.Brands.Public;

[Route("api/brands")]
[ApiController]
public class BrandsController(ListBrandsQuery listBrands, GetBrandBySlugQuery getBrandBySlug) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var response = await listBrands.ExecuteAsync(false, page, pageSize);
        return Ok(response);
    }
    
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var brand = await getBrandBySlug.ExecuteAsync(slug);
        return brand == null
            ? NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Brand not found",
                Detail = $"Brand with slug {slug} does not exist"
            })
            : Ok(brand);
    }
}
