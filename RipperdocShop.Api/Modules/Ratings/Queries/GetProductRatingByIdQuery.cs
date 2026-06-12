using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Ratings.Queries;

public class GetProductRatingByIdQuery(ApplicationDbContext context, IMapper mapper)
{
    public async Task<ProductRatingDto?> ExecuteAsync(Guid id)
    {
        var rating = await context.ProductRatings
            .Include(r => r.Product)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        return mapper.Map<ProductRatingDto>(rating);
    }
}
