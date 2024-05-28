import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { StreamOptionUpdateDto } from '../contracts/stream-option-update-dto';
import { Subject, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class StreamOptionService {
  readonly optionUpdate$ = new Subject<void>();
  private readonly httpClient = inject(HttpClientService);

  updateOption(streamOptionUpdateDto: StreamOptionUpdateDto) {
    const formData = Object.entries(streamOptionUpdateDto).reduce(
      (formData, [key, value]) => {
        formData.append(key, value);
        return formData;
      },
      new FormData()
    );

    return this.httpClient
      .patch(
        { controller: 'stream-options', action: 'title-description' },
        formData
      )
      .pipe(tap(() => this.optionUpdate$.next()));
  }
}
