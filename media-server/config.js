const httpConfig = {
  port: 8000, // HTTP port
  allow_origin: "*", // Allow requests from any origin (you may restrict this as needed)
  mediaroot: "./media", // Directory where the server will look for media files
};

const rtmpConfig = {
  port: 1935, // RTMP port, 1935 is the default port for RTMP
  chunk_size: 60000, // The size in bytes of the chunks into which the media file will be divided
  gop_cache: true, // If true, the server will use a GOP (Group of Pictures) cache. This will improve the efficiency of RTMP streaming but will also increase memory usage
  ping: 10, // Ping interval in seconds. This will send a ping message to the client to check if the connection is alive
  ping_timeout: 60, // Ping timeout in seconds
};

const transformationConfig = {
  ffmpeg: "./ffmpeg",
  tasks: [
    {
      app: "live",
      hls: true,
      hlsFlags: "[hls_time=2:hls_list_size=3:hls_flags=delete_segments]",
      hlsKeep: false,
    },
  ],
  MediaRoot: "./media",
};

export const config = {
  http: httpConfig,
  rtmp: rtmpConfig,
  trans: transformationConfig,
};

// export default config;
