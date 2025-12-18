using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Infrastructure.Services;

public interface IFileService
{
    Task<string> SaveProductImageAsync(IFormFile file);
    Task<bool> DeleteProductImageAsync(string filePath);
    bool IsValidImageFile(IFormFile file);
}

public class FileService : IFileService
{
    private readonly string _imagesPath;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    public FileService(IWebHostEnvironment environment)
    {
        _imagesPath = Path.Combine(environment.WebRootPath, "images", "products");
        
        // Ensure directory exists
        if (!Directory.Exists(_imagesPath))
        {
            Directory.CreateDirectory(_imagesPath);
        }
    }

    public async Task<string> SaveProductImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty or null.");
        }

        if (!IsValidImageFile(file))
        {
            throw new InvalidOperationException("Invalid image file format or size.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_imagesPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/products/{fileName}";
    }

    public Task<bool> DeleteProductImageAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return Task.FromResult(false);
        }

        try
        {
            var fullPath = filePath.StartsWith("/")
                ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'))
                : filePath;

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public bool IsValidImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return false;
        }

        if (file.Length > MaxFileSize)
        {
            return false;
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }
}

