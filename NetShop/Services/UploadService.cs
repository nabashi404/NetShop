using FluentResults;
using Microsoft.AspNetCore.Components.Forms;
using NetShop.Interfaces;
using NetShop.Models;

namespace NetShop.Services;

public class UploadService(IWebHostEnvironment environment) : IUploadService
{
    private readonly string _webRootPath = environment.WebRootPath;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public async Task<Result<UploadResponse>> UploadImageAsync(IBrowserFile file)
    {
        if (file == null || file.Size == 0) return Result.Fail<UploadResponse>("File is empty");

        var extension = Path.GetExtension(file.Name).ToLower();

        if (!_allowedExtensions.Contains(extension)) return Result.Fail<UploadResponse>("Invalid file type");

        var uploadPath = Path.Combine(_webRootPath, "images");
        Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var path = Path.Combine(uploadPath, fileName);

        using var stream = file.OpenReadStream();
        using var fileStream = new FileStream(path, FileMode.Create);
        
        await stream.CopyToAsync(fileStream);

        var uploadResponse = new UploadResponse { Url = $"images/{fileName}" };

        return Result.Ok(uploadResponse);
    }
}
