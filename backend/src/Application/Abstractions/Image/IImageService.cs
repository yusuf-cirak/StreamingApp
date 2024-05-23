namespace Application.Abstractions.Image;

public interface IImageService
{
    Task<string> UploadImageAsync(string id, IFormFile file, string folderName);
    Task<bool> DeleteImageAsync(string id, string folderName);
}