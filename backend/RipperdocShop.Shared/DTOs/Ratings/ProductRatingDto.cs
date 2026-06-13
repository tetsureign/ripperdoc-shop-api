namespace RipperdocShop.Shared.DTOs.Ratings;

public class ProductRatingDto
{
    public Guid Id { get; set; }
    public int Score { get; set; }

    public string? Comment { get; set; }

    public string ProductSlug { get; set; } = string.Empty;

    public Guid UserId { get; set; }
}
