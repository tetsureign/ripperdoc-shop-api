using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Ratings.Commands;
using RipperdocShop.Api.Modules.Ratings.Queries;
using RipperdocShop.Shared.DTOs;
// ReSharper disable ConvertToPrimaryConstructor

namespace RipperdocShop.Api.Modules.Ratings.Customer;

[Route("api/ratings")]
[ApiController]
public class ProductRatingsController : ControllerBase
{
    private readonly CreateProductRatingCommand _createRating;
    private readonly GetProductRatingByIdQuery _getRatingById;
    private readonly ListProductRatingsByProductSlugQuery _listRatingsByProductSlug;
    private readonly UpdateProductRatingCommand _updateRating;

    public ProductRatingsController(
        CreateProductRatingCommand createRating,
        GetProductRatingByIdQuery getRatingById,
        ListProductRatingsByProductSlugQuery listRatingsByProductSlug,
        UpdateProductRatingCommand updateRating)
    {
        _createRating = createRating;
        _getRatingById = getRatingById;
        _listRatingsByProductSlug = listRatingsByProductSlug;
        _updateRating = updateRating;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public async Task<IActionResult> Create(ProductRatingCreateDto createDto)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                throw new Exception("Not logged in.");
            
            var rating = await _createRating.ExecuteAsync(createDto, userId);
            
            return CreatedAtAction(nameof(GetById), new { id = rating?.Id }, rating);
        }
        catch (Exception e)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Could not create rating",
                Detail = e.Message
            });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var rating = await _getRatingById.ExecuteAsync(id);
        return rating == null
            ? NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Rating not found",
                Detail = $"Rating with ID {id} does not exist"
            })
            : Ok(rating);
    }

    [HttpGet("by-product/{slug}")]
    public async Task<IActionResult> GetByProductSlug(string slug, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _listRatingsByProductSlug.ExecuteAsync(slug, false, page, pageSize);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public async Task<IActionResult> Update(Guid id, ProductRatingCreateDto createDto)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                throw new Exception("Not logged in.");
            
            var rating = await _updateRating.ExecuteAsync(id, createDto, userId);
            if (rating == null)
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
