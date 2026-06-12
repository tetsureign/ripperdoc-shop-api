using System.ComponentModel.DataAnnotations;

namespace RipperdocShop.Web.Models.ViewModels;

public class ProductRatingCreateViewModel
{
    [Range(1, 5)]
    public int Score { get; set; }

    public string? Comment { get; set; }

    [Required]
    public Guid ProductId { get; set; }
}
