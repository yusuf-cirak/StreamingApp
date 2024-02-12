import { Body, Controller, Post, Res, UseInterceptors } from "@nestjs/common";
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
  endStream(@Body() streamInfo: any, @Res() res: Response) {
    return this.streamProxyService.endStream(streamInfo.name).pipe(
      tap(() => of(res.status(200).send())),
      catchError((err) => of(res.status(401).send(err))),
    );
  }
}
