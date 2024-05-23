namespace Application.Features.StreamOptions.Services;

public interface IStreamOptionService : IDomainService<StreamOption>
{
    Task<Result<StreamOption, Error>> GetStreamOptionAsync(Guid streamerId,
        CancellationToken cancellationToken = default);


    Task UpdateStreamOptionCacheAndSendNotificationAsync(StreamOption streamOption,
        CancellationToken cancellationToken =
            default);


    Task<string> UploadStreamThumbnailImageAsync(StreamOption streamOption, IFormFile? file,
        string existingThumbnailUrl);
}