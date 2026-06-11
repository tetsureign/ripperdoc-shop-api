using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Shared.DTOs;
// ReSharper disable ConvertToPrimaryConstructor

namespace RipperdocShop.Api.Modules.Ratings.Customer;

[Route("api/ratings")]
[ApiController]
public class ProductRatingsController : ControllerBase
{
    private readonly ICustomerProductRatingService _productRatingService;

    public ProductRatingsController(ICustomerProductRatingService productRatingService)
    {
        _productRatingService = productRatingService;
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
            
            var rating = await _productRatingService.CreateAsync(createDto, userId);
            
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
        var rating = await _productRatingService.GetByIdAsync(id);
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
        var response = await _productRatingService.GetByProductSlugAsync(slug, false, page, pageSize);
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
            
            var rating = await _productRatingService.UpdateAsync(id, createDto, userId);
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
