using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Images.Commands;

namespace RipperdocShop.Api.Modules.Images.Admin;

[Route("api/admin/images")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class ImageUploadController(UploadImageCommand uploadImage) : ControllerBase
{
    [HttpPost("upload")]
    [RequestSizeLimit(UploadImageCommand.MaxFileSize)]
    public async Task<IActionResult> Upload(IFormFile image)
    {
        var imageUrl = await uploadImage.ExecuteAsync(image);
        return Ok(new { imageUrl });
    }
}
