import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Story } from '../+models/story';
import { StoryApiService } from './story.api';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {
  private _stories: BehaviorSubject<Story[]> = new BehaviorSubject([]);

  constructor(private storyApiService: StoryApiService) {
    this.storyApiService.GetStories(' ').pipe(take(1)).subscribe(res => this._stories.next(res.map(o => {
      o.topicsArr = o.topics.split(';');
      return o;
    })))
  }

  public get stories$(): Observable<Story[]> {
    return this._stories.asObservable();
  }

  public addStory(story: Story): void {
    this._stories.next([...this._stories.getValue(), story]);
  }
}
