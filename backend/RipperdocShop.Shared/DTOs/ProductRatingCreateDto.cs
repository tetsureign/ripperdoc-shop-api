namespace RipperdocShop.Shared.DTOs;

public class ProductRatingCreateDto
{
    public int Score { get; set; }

    public string? Comment { get; set; }

    public string ProductSlug { get; set; } = string.Empty;
}
