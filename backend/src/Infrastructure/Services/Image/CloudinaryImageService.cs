using Application.Abstractions.Image;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;

namespace Infrastructure.Services.Image;

public sealed class CloudinaryImageService : ICloudinaryImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadImageAsync(string id, IFormFile file, string folderName)
    {
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(id, stream),
            // Transformation = new Transformation().Height(50).Width(50).Crop("fill").Gravity("face"),
            Folder = folderName
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult.PublicId.Split('/')[^1];
    }

    public async Task<bool> DeleteImageAsync(string id, string folderName)
    {
        var deleteParams = new DeletionParams($"{folderName}/{id}")
        {
            ResourceType = ResourceType.Image,
            Type = "upload"
        };

        var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

        return deleteResult.Error == null;
    }
}