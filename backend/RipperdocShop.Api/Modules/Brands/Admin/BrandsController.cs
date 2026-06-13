using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Brands.Commands;
using RipperdocShop.Api.Modules.Brands.Queries;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Shared.DTOs.Brands;

namespace RipperdocShop.Api.Modules.Brands.Admin;

[Route("api/admin/brands")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class BrandsController(
    ListAdminBrandsQuery listAdminBrands,
    GetBrandByIdQuery getBrandById,
    CreateBrandCommand createBrand,
    UpdateBrandCommand updateBrand,
    SoftDeleteBrandCommand softDeleteBrand,
    RestoreBrandCommand restoreBrand,
    DeleteBrandPermanentlyCommand deleteBrandPermanently) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await listAdminBrands.ExecuteAsync(includeDeleted, page, pageSize));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(BrandCreateDto createDto)
    {
        var brand = await createBrand.ExecuteAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var brand = await getBrandById.ExecuteAsync(id);
        return brand == null
            ? NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Brand not found",
                Detail = $"Brand with ID {id} does not exist"
            })
            : Ok(brand);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, BrandCreateDto createDto)
    {
        var brand = await updateBrand.ExecuteAsync(id, createDto);
        if (brand == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Brand with ID {id} does not exist"
            });

        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var brand = await softDeleteBrand.ExecuteAsync(id);
        if (brand == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Brand with ID {id} does not exist"
            });

        return NoContent();
    }


    [HttpPost("{id:guid}/restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Restore(Guid id)
    {
        var brand = await restoreBrand.ExecuteAsync(id);
        if (brand == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Brand with ID {id} does not exist"
            });

        return NoContent();
    }

    [HttpDelete("{id:guid}/hard")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePermanently(Guid id)
    {
        var brand = await deleteBrandPermanently.ExecuteAsync(id);
        if (brand == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Brand with ID {id} does not exist"
            });

        return NoContent();
    }
}
