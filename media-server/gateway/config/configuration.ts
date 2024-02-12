import { StreamApiOption } from "src/shared/models/stream-api.option";
import { RedisOption } from "./../src/shared/models/redis.option";
export default () => ({
  redis: {
    host: process.env.REDIS_HOST || "localhost",
    port: process.env.REDIS_PORT || 6379,
    // password: process.env.REDIS_PASSWORD || "password",
  } as RedisOption,
  streamApi: {
    baseUrl: process.env.STREAM_API_BASE_URL || "http://localhost:8080/api",
    key: process.env.STREAM_API_KEY || "key",
  } as StreamApiOption,
});
