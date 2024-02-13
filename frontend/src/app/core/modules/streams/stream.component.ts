import { OfflineStreamComponent } from './components/offline-stream/offline-stream.component';
import { ActivatedRoute } from '@angular/router';
import { Component, computed, inject } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { LiveStreamComponent } from './components/live-stream/live-stream.component';
import { StreamState } from './models/stream-state';
import { StreamFacade } from './services/stream.facade';
import { NotFoundStreamComponent } from './components/not-found-stream/not-found-stream.component';

@Component({
  selector: 'app-stream',
  standalone: true,
  templateUrl: './stream.component.html',
  imports: [NgTemplateOutlet, LiveStreamComponent, OfflineStreamComponent,NotFoundStreamComponent],
})
export class StreamComponent {
  readonly route = inject(ActivatedRoute);

  readonly streamFacade = inject(StreamFacade);

  streamState = computed(()=>this.streamFacade.streamState())

  liveStream = computed(()=>this.streamFacade.liveStream())

  ngOnInit(){
    console.log(this.streamState())
  }


}
