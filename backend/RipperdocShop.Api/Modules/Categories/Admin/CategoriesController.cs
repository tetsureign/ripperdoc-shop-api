using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Categories.Commands;
using RipperdocShop.Api.Modules.Categories.Queries;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Categories.Admin;

[Route("api/admin/categories")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class CategoriesController(
    ListAdminCategoriesQuery listAdminCategories,
    GetCategoryByIdQuery getCategoryById,
    CreateCategoryCommand createCategory,
    UpdateCategoryCommand updateCategory,
    SoftDeleteCategoryCommand softDeleteCategory,
    RestoreCategoryCommand restoreCategory,
    DeleteCategoryPermanentlyCommand deleteCategoryPermanently)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await listAdminCategories.ExecuteAsync(includeDeleted, page, pageSize));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CategoryCreateDto createDto)
    {
        var category = await createCategory.ExecuteAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await getCategoryById.ExecuteAsync(id);

        if (category == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Category not found",
                Detail = $"Category with ID {id} does not exist"
            });

        return Ok(category);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, CategoryCreateDto createDto)
    {
        var category = await updateCategory.ExecuteAsync(id, createDto);

        if (category == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Category with ID {id} does not exist"
            });

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var category = await softDeleteCategory.ExecuteAsync(id);

        if (category == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Category with ID {id} does not exist"
            });

        return NoContent();
    }

    [HttpPost("{id:guid}/restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Restore(Guid id)
    {
        var category = await restoreCategory.ExecuteAsync(id);

        if (category == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Category with ID {id} does not exist"
            });

        return NoContent();
    }

    [HttpDelete("{id:guid}/hard")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePermanently(Guid id)
    {
        var category = await deleteCategoryPermanently.ExecuteAsync(id);

        if (category == null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"Category with ID {id} does not exist"
            });

        return NoContent();
    }
}
