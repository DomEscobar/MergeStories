import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TopicsService {

  topics: string[] = new Array();

  constructor() {
    this.topics.push(...['Frontend', 'Angular', 'Backend', '.Net', 'Javascript']);
  }
}
