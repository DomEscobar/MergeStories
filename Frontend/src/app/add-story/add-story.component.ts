import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { from, fromEvent, throwError } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { TopicsService } from '../+services/topics.service';
import { Story } from '../+models/story';
import { StoriesService } from '../+services/stories.service';
import { StoryApiService } from '../+services/story.api';

@Component({
  selector: 'app-add-story',
  templateUrl: './add-story.component.html',
  styleUrls: ['./add-story.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddStoryComponent {

  @Output()
  onCancel: EventEmitter<void> = new EventEmitter();

  public imageCase: string;
  public imageSolution: string;
  public selectedTopics: string[] = new Array();

  constructor(
    private readonly storyApiService: StoryApiService,
    private readonly changedetector: ChangeDetectorRef,
    public topicsService: TopicsService,
    public storiesService: StoriesService,
  ) {
    this.addClipboardListener();
  }

  ngAfterViewInit(): void {
    this.changedetector.markForCheck();
  }

  public cancel() {
    this.onCancel.emit();
  }

  public async postStory() {
    try {

      const story = new Story();
      story.imgCase = this.imageCase;
      story.imgSolution = this.imageSolution;
      story.topics = this.selectedTopics.join(';');
      story.date = new Date();
      const result = await this.storyApiService.AddStory(story).toPromise();
      result.topicsArr = result.topics?.split(';');

      this.storiesService.addStory(result);
      this.onCancel.emit();
    } catch {
      window.alert('Error while trying to create the story')
    }
  }

  public selectTopic(topic): void {

    if (this.isSelectedTopic(topic)) {
      this.selectedTopics = this.selectedTopics.filter(o => o != topic);
      return;
    }

    this.selectedTopics.push(topic);
  }

  public isSelectedTopic(topic: string): boolean {
    return this.selectedTopics.includes(topic);
  }

  public get isImageCaseEmpty(): boolean {
    return this.imageCase == undefined;
  }

  public get isImageSolutionEmpty(): boolean {
    return this.imageSolution == undefined;
  }

  public reset(): void {
    this.imageCase = undefined;
    this.imageSolution = undefined;
  }

  private addClipboardListener(): void {
    fromEvent(document, 'paste').subscribe(async (event: ClipboardEvent) => {
      const items = event.clipboardData;

      const blob = await from(items.items).pipe(
        filter(o => o.type.includes('image'),
        )).toPromise();

      if (blob == undefined) {
        window.alert('No image at your clipboard');
        return;
      }

      const reader = new FileReader();
      reader.onload = (evt: any) => {

        if (this.isImageCaseEmpty) {
          this.imageCase = evt.target.result;
        } else {
          this.imageSolution = evt.target.result;
        }

        this.changedetector.markForCheck();
      };
      reader.readAsDataURL(blob.getAsFile());

      event.preventDefault();
    });
  }
}
