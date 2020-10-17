import { Component } from '@angular/core';
import { Loading } from './Loading';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css']
})
export class LoadingComponent
{

  isLoading = false;

  constructor()
  {
    Loading.onLoad.subscribe(isLoading =>
    {
      this.isLoading = isLoading;
    });
  }

  private hide()
  {
    this.isLoading = false;
  }
}
