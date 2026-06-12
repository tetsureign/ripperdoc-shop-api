using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Shared.DTOs;

namespace RipperdocShop.Api.Modules.Ratings.Queries;

public class ListProductRatingsByProductSlugQuery(ApplicationDbContext context, IMapper mapper)
{
    public async Task<PaginatedProductRatingResponse> ExecuteAsync(string slug, bool includeDeleted, int page,
        int pageSize)
    {
        var query = context.ProductRatings
            .Include(r => r.User)
            .Include(r => r.Product)
            .Where(r => r.Product.Slug == slug)
            .Where(r => includeDeleted || r.DeletedAt == null);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var ratings = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedProductRatingResponse
        {
            Ratings = mapper.Map<IEnumerable<ProductRatingDto>>(ratings),
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}
