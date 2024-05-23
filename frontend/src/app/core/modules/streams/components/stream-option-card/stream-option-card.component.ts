import { Component, computed, inject, signal } from '@angular/core';
import { PencilIcon } from '../../../../../shared/icons/pencil-icon';
import { ModalComponent } from '../../../../../shared/components/modal/modal.component';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { InputComponent } from '../../../../../shared/components/input/input.component';
import { LoadingIcon } from '../../../../../shared/icons/loading-icon';
import { StreamFacade } from '../../services/stream.facade';
import { minLength } from '../../../../validators';
import { catchError, of, tap } from 'rxjs';
import { TrashIcon } from '../../../../../shared/icons/trash-icon';
import { StreamOptionService } from '../../services/stream-option.service';
import { StreamOptionUpdateDto } from '../../contracts/stream-option-update-dto';
import { ToastrService } from 'ngx-toastr';
import { StreamOptions } from '../../../../models/stream-options';
import { ImageService } from '../../../../services/image.service';

@Component({
  selector: 'app-stream-option-card',
  standalone: true,
  imports: [
    PencilIcon,
    ModalComponent,
    ReactiveFormsModule,
    InputComponent,
    LoadingIcon,
    TrashIcon,
  ],
  templateUrl: './stream-option-card.component.html',
})
export class StreamOptionCardComponent {
  readonly streamFacade = inject(StreamFacade);

  readonly #fb = inject(NonNullableFormBuilder);

  readonly toastr = inject(ToastrService);

  readonly streamOptionService = inject(StreamOptionService);

  readonly imageService = inject(ImageService);

  modalVisible = signal(false);
  formSubmitted = signal(false);

  streamerId = computed(() => this.streamFacade.liveStream().user.id);

  option = computed(() => this.streamFacade.liveStream().streamOption);

  form = this.#fb.group({
    streamTitle: [this.option().title, [minLength(4, 'Stream title')]],
    streamDescription: [
      this.option().description,
      [minLength(4, 'Stream description')],
    ],
    thumbnail: !!this.option().thumbnailUrl
      ? this.imageService.getThumbnailUrl(this.option().thumbnailUrl)
      : ('' as File | string),
  });

  thumbnailUrl = signal<string>(this.form.value.thumbnail as string);

  isThumbnailUpdated = signal(false);

  submit() {
    if (!this.form.dirty && this.form.touched) {
      this.toastr.info('You did not make any changes');
      return;
    }
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.formSubmitted.set(true);

    const streamOptionUpdateDto = {
      streamerId: this.streamerId(),
      thumbnailUrl: this.isThumbnailUpdated() ? '' : this.option().thumbnailUrl,
      ...this.form.value,
      thumbnail: this.isThumbnailUpdated() ? this.form.value.thumbnail : null,
    };

    this.streamOptionService
      .updateOption(streamOptionUpdateDto as unknown as StreamOptionUpdateDto)
      .pipe(
        tap(() => {
          const streamOptions = {
            ...this.option(),
            title: streamOptionUpdateDto.streamTitle,
            description: streamOptionUpdateDto.streamDescription,
          } as StreamOptions;
          let streamState = this.streamFacade.streamState();
          streamState!.stream!.streamOption = streamOptions;
          this.streamFacade.setStream(streamState);
          this.formSubmitted.set(false);
          this.modalVisible.set(false);
          this.isThumbnailUpdated.set(false);
          this.toastr.success('Stream option updated successfully');
        }),
        catchError(() => {
          this.formSubmitted.set(false);
          this.toastr.error('Failed to update stream option');
          return of(undefined);
        })
      )
      .subscribe();
  }

  setFile(file?: File | string) {
    this.isThumbnailUpdated.set(true);
    setTimeout(() => {
      this.form.patchValue({ thumbnail: file });
      this.setUrlForThumbnail(file!);
    });
  }

  private setUrlForThumbnail(image: File | string) {
    if (!image) {
      this.thumbnailUrl.set('');
      return;
    }
    const reader = new FileReader();
    reader.onload = (e) => {
      this.thumbnailUrl.set(e.target?.result as string);
    };
    reader.readAsDataURL(image as File);
  }
}
