import { Injectable } from "@nestjs/common";
import { ConfigService } from "@nestjs/config";
import { RedisOption } from "../models/redis.option";
import * as IORedis from "ioredis";

@Injectable()
export class RedisService {
  constructor(private configService: ConfigService) {
    const options = this.configService.get<RedisOption>("redis");
    // Create a Redis client
    this.redisClient = new IORedis.Redis({
      ...options,
    });

    // Handle errors
    this.redisClient.on("error", (err) => {
      console.error(`Redis error: ${err}`);
    });
  }
  private readonly redisClient: IORedis.Redis;

  async get(key: string): Promise<string | null> {
    return await this.redisClient.get(key);
  }
}
