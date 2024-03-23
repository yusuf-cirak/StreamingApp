import { Body, Controller, Post, Res } from "@nestjs/common";
import { PublishStreamDto } from "./models/publish-stream-dto";
import { catchError, of, tap } from "rxjs";
import { StreamProxyService } from "./services/stream-proxy.service";
import { Response } from "express";

@Controller("api/streams")
export class StreamController {
  constructor(private streamProxyService: StreamProxyService) {}

  @Post("publish")
  publishStream(@Body() streamInfo: PublishStreamDto, @Res() res: Response) {
    return this.streamProxyService.publishStream(streamInfo.name).pipe(
      tap((response) => {
        return of(res.status(200).send(response.data));
      }),
      catchError((err) => {
        return of(res.status(401).send(err));
      }),
    );
  }

  @Post("end")
  endStream(@Body() streamInfo: PublishStreamDto, @Res() res: Response) {
    return this.streamProxyService.endStream(streamInfo.name).pipe(
      tap(() => {
        res.status(200).send();
      }),
      catchError((err) => {
        res.status(401).send(err);
        return of(err);
      }),
    );
  }
}
