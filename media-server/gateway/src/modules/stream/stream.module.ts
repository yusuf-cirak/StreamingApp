/* eslint-disable prettier/prettier */
import { Module } from "@nestjs/common";
import { StreamController } from "./stream.controller";
import { HttpModule } from "@nestjs/axios";
import { StreamProxyService } from "./services/stream-proxy.service";

@Module({
  imports: [HttpModule],
  controllers: [StreamController],
  providers: [StreamProxyService],
})
export class StreamModule {}
