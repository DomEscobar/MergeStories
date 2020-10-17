import { Component, Injectable, Injector } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { Loading } from '../static/loading/Loading';

export class BaseService
{
  protected http: HttpClient;
  protected apiUrl = environment.APIURL;

  constructor(private injector: Injector)
  {
    this.http = this.injector.get(HttpClient);
  }

  get<T>(path: string, param: any = '', showError = true, showLoading = true, apiUrl = this.apiUrl): Observable<any | T>
  {
    if (showLoading)
    {
      Loading.show();
    }

    const observer = this.http.get<T>(apiUrl + path + '/' + param, {}).pipe(

      catchError(err =>
      {
        if (showError)
        {
          return of([]);
        }

        return throwError('fail');
      }),
      finalize(() =>
      {

        if (showLoading)
        {
          Loading.hide();
        }
      })
    );

    return observer;
  }

  post<T>(path: string, creds: any = '', showError = true, showLoading = true): Observable<any | T>
  {
    Loading.show();

    const observer = this.http.post<T>(this.apiUrl + path, creds, {}).pipe(

      catchError(err =>
      {
        if (showError)
        {
          return of([]);
        }

        return throwError('');

      }),
      finalize(() => Loading.hide())
    );

    return observer;
  }


  put<T>(path: string, creds: any = '', showError = true, showLoading = true): Observable<any | T>
  {
    Loading.show();

    const observer = this.http.put<T>(this.apiUrl + path, creds, {}).pipe(

      catchError(err =>
      {
        if (showError)
        {
          return of([]);
        }

        return throwError('');
      }),
      finalize(() => Loading.hide())
    );

    return observer;
  }

  delete(path: string, id: any = ''): Observable<any>
  {
    Loading.show();

    const observer = this.http.delete(this.apiUrl + path, {}).pipe(
      catchError(err =>
      {
        return throwError('');
      }),
      finalize(() => Loading.hide())
    );

    return observer;
  }
}
