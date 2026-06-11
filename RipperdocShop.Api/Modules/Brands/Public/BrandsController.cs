using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Brands.Customer;

namespace RipperdocShop.Api.Modules.Brands.Public;

[Route("api/brands")]
[ApiController]
public class BrandsController(ICustomerBrandService brandService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var response = await brandService.GetAllAsync(false, page, pageSize);
        return Ok(response);
    }
    
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var brand = await brandService.GetBySlugAsync(slug);
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
