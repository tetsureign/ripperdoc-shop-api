namespace RipperdocShop.Api.Modules.Images.Commands;

public class UploadImageCommand(IWebHostEnvironment environment)
{
    private const int FileSizeMb = 5;
    public const long MaxFileSize = FileSizeMb * 1024 * 1024;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public async Task<string> ExecuteAsync(IFormFile image, string folder = "images")
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("Image is empty.");

        if (image.Length > MaxFileSize)
            throw new ArgumentException("Image is too large (max 5MB).");

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            throw new ArgumentException("Unsupported image format.");

        var uploadsFolder = Path.Combine(environment.WebRootPath, folder);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return $"/{folder}/{fileName}";
    }
}
