import { Module } from "@nestjs/common";
import { StreamModule } from "./modules/stream/stream.module";
import { ConfigModule } from "@nestjs/config";
import configuration from "config/configuration";
import { APP_INTERCEPTOR } from "@nestjs/core";

@Module({
  imports: [
    ConfigModule.forRoot({ load: [configuration], isGlobal: true }),
    StreamModule,
  ],
  controllers: [],
})
export class AppModule {}
