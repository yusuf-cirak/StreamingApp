import { Component, computed, inject, input } from '@angular/core';
import { ChatSidebarComponent } from '../../../chat-sidebar/chat-sidebar.component';

@Component({
  selector: 'app-not-found-stream',
  standalone: true,
  templateUrl: './not-found-stream.component.html',
  imports: [ChatSidebarComponent],
})
export class NotFoundStreamComponent {

}
