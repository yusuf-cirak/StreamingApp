import { RedisOption } from "./../src/shared/models/redis.option";
export default () => ({
  redis: {
    host: process.env.REDIS_HOST || "localhost",
    port: process.env.REDIS_PORT || 6379,
    // password: process.env.REDIS_PASSWORD || "password",
  } as RedisOption,
  streamApiUrl: process.env.STREAM_API_URL || "http://localhost:5117/api",
});
