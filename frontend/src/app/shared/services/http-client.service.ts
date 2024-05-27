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

      url += requestParameter.routeParams
        ? requestParameter.routeParams.reduce(
            (acc, curr) => `${acc}/${curr}`,
            ''
          )
        : '';
    }

    const queryStringsLength = requestParameter?.queryStrings?.length ?? 0;

    const queryString = requestParameter.queryStrings?.reduce(
      (queryString, current, index) => {
        queryString += `${current.queryName}=${current.query}${
          index !== 0 || index !== queryStringsLength - 1 ? '&' : ''
        }`;
        return queryString;
      },
      ''
    );

    url += queryString?.length ? '?' + queryString : '';

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

  patch<T>(
    requestParameter: Partial<RequestParameter>,
    body: Partial<T> | any
  ): Observable<T> {
    return this._httpClient.patch<T>(this.url(requestParameter), body, {
      headers: requestParameter.headers,
      withCredentials: requestParameter.withCredentials,
    });
  }

  delete(requestParameter: Partial<RequestParameter>, body: unknown) {
    return this._httpClient.delete(this.url(requestParameter), {
      headers: requestParameter.headers,
      body,
    });
  }
}

export interface RequestParameter {
  // API controller, action
  controller: string; // API Controller
  action?: string;

  // Http header,query string, baseUrl
  queryStrings: QueryString[];
  routeParams: string[];
  headers: HttpHeaders;
  baseUrl: string;

  // Other services (might have different routes)
  fullEndPoint: string; // Dış dünyayla iletişime geçmemiz gerekebilir, dış dünyadaki servisin route'ı bizimkiyle uyuşmayabilir.
  withCredentials: boolean;
}

export interface QueryString {
  queryName: string;
  query: string;
}
