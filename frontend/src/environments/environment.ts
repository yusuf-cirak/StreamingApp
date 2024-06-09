export const environment = {
  apiUrl: 'http://localhost:5117/api',
  hlsUrl: 'http://localhost:8080/hls',
  rtmpUrl: 'rtmp://localhost/live',
  streamHubUrl: 'http://localhost:5117/_stream',
  defaultProfileImageApiUrl: 'https://ui-avatars.com/api',
  cloudinary: {
    publicKey: 'dhcu4h56y',
    baseUrl:
      'https://res.cloudinary.com/dhcu4h56y/image/upload/f_auto,q_auto/v1',
    folderNames: {
      streamThumbnails: 'stream_thumbnails',
      profileImages: 'profile_images',
    },
  },
};
