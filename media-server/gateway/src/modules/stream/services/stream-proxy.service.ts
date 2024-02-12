import { ConfigService } from "@nestjs/config";
import { HttpService } from "@nestjs/axios";
import { Injectable } from "@nestjs/common";
import { AxiosResponse } from "axios";
import { Observable } from "rxjs";
import { StreamApiOption } from "src/shared/models/stream-api.option";

@Injectable()
export class StreamProxyService {
  private readonly streamApiOptions: StreamApiOption;

  constructor(
    private configService: ConfigService,
    private httpService: HttpService,
  ) {
    this.streamApiOptions =
      this.configService.get<StreamApiOption>("streamApi");
  }

  publishStream(streamKey: string): Observable<AxiosResponse<string, Error>> {
    return this.httpService.post<string>(
      `${this.streamApiOptions.baseUrl}/streams`,
      {
        streamKey,
      },
      { headers: { "X-Api-Key": this.streamApiOptions.key } },
    );
  }

  endStream(streamKey: string): Observable<AxiosResponse<any, Error>> {
    return this.httpService.patch(
      `${this.streamApiOptions.baseUrl}/streams`,
      { streamKey },
      { headers: { "X-Api-Key": this.streamApiOptions.key } },
    );
  }
}
