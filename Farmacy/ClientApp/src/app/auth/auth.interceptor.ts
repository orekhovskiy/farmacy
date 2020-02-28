import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {  

  constructor(private router: Router) {}  

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem('access-token') }`
      }
    });
    return next.handle(request).pipe(catchError(x => this.handleAuthError(x)));
  }

  private handleAuthError(err: HttpErrorResponse):Observable<any> {
    if (err.status === 401 ) {
      localStorage.clear();
      this.router.navigateByUrl('/error?status=401&message=Ошибка авторизации');
      return of(err.message);
    }
    if (err.status === 403) {
      this.router.navigateByUrl('/error?status=403&message=Отказано в доступе');
      return of(err.message);
    }
    return throwError(err);
  }
}