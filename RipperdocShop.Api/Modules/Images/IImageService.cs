namespace RipperdocShop.Api.Modules.Images;

public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile image, string folder = "images");
}
