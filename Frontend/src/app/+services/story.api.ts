import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from '../core/baseService';
import { Story } from '../+models/story';

@Injectable({
  providedIn: 'root'
})
export class StoryApiService extends BaseService {
  readonly APIURL = 'Story';

  constructor(injector: Injector) {
    super(injector);
  }

  AddStory(story: Story): Observable<Story> {
    return this.put<Story>(`${ this.APIURL }`, story, false);
  }

  GetStories(searchValue: string): Observable<Story[]> {
    return this.get<Story[]>(`${ this.APIURL }/search/${ searchValue }`);
  }
}
