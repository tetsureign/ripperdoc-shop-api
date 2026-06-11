using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RipperdocShop.Api.Modules.Ratings.Core;

namespace RipperdocShop.Api.Modules.Ratings.Admin;

[Route("api/admin/ratings")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class ProductRatingsController(
    IAdminProductRatingService productRatingService,
    IProductRatingCoreService productRatingCoreService) : ControllerBase
{
    [HttpGet("by-product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId, [FromQuery] bool includeDeleted = false,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (ratings, totalCount, totalPages) =
            await productRatingCoreService.GetByProductAsync(productId, includeDeleted, page, pageSize);

        var response = new AdminProductRatingResponse()
        {
            Ratings = ratings,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return response.Ratings.IsNullOrEmpty() ? NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Product not found",
            Detail = $"Product with ID {productId} does not exist"
        }) : Ok(response);
    }

    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, [FromQuery] bool includeDeleted = false, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (ratings, totalCount, totalPages) =
            await productRatingCoreService.GetByUserAsync(userId, includeDeleted, page, pageSize);

        var response = new AdminProductRatingResponse()
        {
            Ratings = ratings,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return response.Ratings.IsNullOrEmpty() ? NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Product not found",
            Detail = $"Product with ID {userId} does not exist"
        }) : Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        try
        {
            var product = await productRatingService.SoftDeleteAsync(id);
            if (product == null)
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found",
                    Detail = $"Rating with ID {id} does not exist"
                });

            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid operation",
                Detail = e.Message
            });
        }
    }

    [HttpPost("{id:guid}/restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Restore(Guid id)
    {
        try
        {
            var product = await productRatingService.RestoreAsync(id);
            if (product == null)
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found",
                    Detail = $"Rating with ID {id} does not exist"
                });

            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid operation",
                Detail = e.Message
            });
        }
    }
}
