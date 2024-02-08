import { ConfigService } from "@nestjs/config";
import { HttpService } from "@nestjs/axios";
import { Injectable } from "@nestjs/common";
import { AxiosResponse } from "axios";
import { Observable } from "rxjs";

@Injectable()
export class StreamProxyService {
  private readonly baseApiUrl: string;

  constructor(
    private configService: ConfigService,
    private httpService: HttpService,
  ) {
    this.baseApiUrl = `${configService.get<string>("streamApiUrl")}/streams`;
    console.log(this.baseApiUrl);
  }

  publishStream(streamKey: string): Observable<AxiosResponse<string, Error>> {
    return this.httpService.post<string>(this.baseApiUrl, { streamKey });
  }

  endStream(streamKey: string): Observable<AxiosResponse<any, Error>> {
    return this.httpService.patch(this.baseApiUrl, { streamKey });
  }
}
