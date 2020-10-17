import { Component } from '@angular/core';
import { StoriesService } from './+services/stories.service';
import { Observable } from 'rxjs';
import { Story } from './+models/story';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public isOnAdd: boolean;
  public stories$: Observable<Story[]>;

  constructor(private storiesService: StoriesService) {
    this.stories$ = this.storiesService.stories$;
  }
}
