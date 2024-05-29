using Application.Abstractions.Image;
using Application.Common.Mapping;
using Application.Contracts.Constants;
using Application.Features.Streams.Services;
using SignalR.Hubs.Stream.Server.Abstractions;

namespace Application.Features.StreamOptions.Services;

public sealed class StreamOptionService : IStreamOptionService
{
    private readonly IEfRepository _efRepository;
    private readonly IStreamService _streamService;
    private readonly IStreamCacheService _streamCacheService;
    private readonly IStreamHubServerService _streamHubServerService;
    private readonly IImageService _imageService;
    private readonly List<GetStreamDto> LiveStreamers;

    public StreamOptionService(IEfRepository efRepository, IStreamCacheService streamCacheService,
        IStreamHubServerService streamHubServerService, IImageService imageService, IStreamService streamService)
    {
        _efRepository = efRepository;
        _streamCacheService = streamCacheService;
        _streamHubServerService = streamHubServerService;
        _imageService = imageService;
        _streamService = streamService;
        LiveStreamers = streamCacheService.LiveStreamers;
    }

    public StreamOption CreateStreamOption(User user)
    {
        // All users are also streamers, so we create a streamer for the user
        var streamerKey = _streamService.GenerateStreamKey(user);
        var streamerTitle = $"{user.Username}'s stream";
        var streamerDescription = $"{user.Username}'s stream description";

        var streamOption = StreamOption.Create(user.Id, streamerKey, streamerTitle, streamerDescription);

        _efRepository.StreamOptions.Add(streamOption);

        return streamOption;
    }

    public async Task<Result<StreamOption, Error>> GetStreamOptionAsync(Guid streamerId,
        CancellationToken cancellationToken = default)
    {
        var streamOption = await
            _efRepository
                .StreamOptions
                .Include(so => so.Streamer)
                .ThenInclude(user => user.Streams)
                .AsTracking()
                .SingleOrDefaultAsync(so => so.Id == streamerId,
                    cancellationToken: cancellationToken);


        return streamOption is not null ? streamOption : StreamOptionErrors.StreamerDoesNotExist;
    }

    public async Task
        UpdateStreamOptionCacheAndSendNotificationAsync(StreamOption streamOption, CancellationToken cancellationToken =
            default)
    {
        var index = LiveStreamers.FindIndex(ls => ls.User.Id == streamOption.Streamer.Id);

        if (index is -1)
        {
            return;
        }

        var currentState = LiveStreamers[index].StreamOption!.Value;

        LiveStreamers[index].StreamOption = streamOption.ToDto(streamKey: currentState.StreamKey);

        var updateCacheTask = _streamCacheService.SetLiveStreamsAsync(LiveStreamers, cancellationToken);
        var sendNotificationTask =
            this.SendChatOptionsUpdatedNotificationAsync(LiveStreamers[index], cancellationToken);

        await Task.WhenAll(updateCacheTask, sendNotificationTask);
    }

    public async Task<string> UploadStreamThumbnailImageAsync(StreamOption streamOption, IFormFile file,
        string existingThumbnailUrl)
    {
        var thumbnailUrl = string.Empty;
        if (file is not null && existingThumbnailUrl.Length is 0)
        {
            thumbnailUrl = await _imageService.UploadImageAsync(streamOption.Streamer.Id.ToString(), file,
                ImageConstants.Folder.StreamThumbnailFolder);
        }
        else if (existingThumbnailUrl.Length > 0)
        {
            thumbnailUrl = streamOption.ThumbnailUrl;
        }
        else if (streamOption.ThumbnailUrl.Length > 0)
        {
            _ = Task.Run(() => _imageService.DeleteImageAsync(streamOption.ThumbnailUrl,
                ImageConstants.Folder.StreamThumbnailFolder));
        }

        return thumbnailUrl;
    }


    private Task SendChatOptionsUpdatedNotificationAsync(GetStreamDto streamDto,
        CancellationToken cancellationToken = default)
    {
        return _streamHubServerService.OnStreamChatOptionsChangedAsync(
            streamDto.StreamOption!.Value.ToStreamChatSettingsDto(), streamDto.User.Username);
    }
}