import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Helper } from '../static/helper';

@Injectable()
export class HeaderInterceptor implements HttpInterceptor {
  constructor() {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('token') ?? Helper.createId();
    localStorage.setItem('token', token);
    if (token != null) {
      return next.handle(req.clone({
        setHeaders: {
          'Authentication': 'AuthenticationToken ' + token,
          'content-type': 'application/json'
        }
      }));
    }
    else {
      return next.handle(req);
    }
  }
}
