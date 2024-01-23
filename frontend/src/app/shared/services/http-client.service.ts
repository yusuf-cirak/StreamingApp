import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class HttpClientService {
  private readonly _httpClient = inject(HttpClient);

  private url(requestParameter: Partial<RequestParameter>, idParam?: string) {
    // Her istekte url ayarlamamız gerek. O yüzden kod tekrarı yapmamak için fonksiyon yazıyoruz.
    let url: string = '';
    if (requestParameter.fullEndPoint) url = requestParameter.fullEndPoint; //
    else {
      url = `${requestParameter.baseUrl ?? environment.apiUrl}/${
        requestParameter.controller
      }`;
      /*
      1. ternany operator : requestParameter'da baseUrl değeri var mı? varsa url'ye ekle yoksa basedeki baseUrl'yi ekle
      */

      url += `${requestParameter.action ? `/${requestParameter.action}` : ''}`;

      /*
      ternany operator : action var mı? varsa /action şeklinde ekle, yoksa bir şey ekleme.
      */

      url += `${idParam ? `/${idParam}` : ''}${
        requestParameter.queryString ? `?${requestParameter.queryString}` : ''
      }`;
      /*
    1. ternany operator: id değeri varsa /id şeklinde endpoint'i genişlet, yoksa boş string dön
    2. ternany operator: queryString varsa ?queryString şeklinde dön, yoksa boş string dön
    */
    }
    return url;
  }

  get<T>(
    requestParameter: Partial<RequestParameter>,
    id?: string
  ): Observable<T> {
    return this._httpClient.get<T>(this.url(requestParameter, id), {
      headers: requestParameter.headers,
      withCredentials: requestParameter.withCredentials,
    });
  }

  post<T>(
    requestParameter: Partial<RequestParameter>,
    body: Partial<T> | any
  ): Observable<T> {
    return this._httpClient.post<T>(this.url(requestParameter), body, {
      headers: requestParameter.headers,
      withCredentials: requestParameter.withCredentials,
      responseType: (requestParameter?.responseType || 'json') as 'json',
    });
  }

  put<T>(
    requestParameter: Partial<RequestParameter>,
    body: Partial<T> | any
  ): Observable<T> {
    return this._httpClient.put<T>(this.url(requestParameter), body, {
      headers: requestParameter.headers,
      withCredentials: requestParameter.withCredentials,
    });
  }
  delete(requestParameter: Partial<RequestParameter>, id: string) {
    return this._httpClient.delete(this.url(requestParameter, id), {
      headers: requestParameter.headers,
    });
  }
}

export interface RequestParameter {
  // API controller, action
  controller: string; // API Controller
  action?: string;

  // Http header,query string, baseUrl
  queryString?: string;
  headers: HttpHeaders;
  baseUrl?: string;

  // Other services (might have different routes)
  fullEndPoint?: string; // Dış dünyayla iletişime geçmemiz gerekebilir, dış dünyadaki servisin route'ı bizimkiyle uyuşmayabilir.
  withCredentials?: boolean;
  responseType: string;
}
