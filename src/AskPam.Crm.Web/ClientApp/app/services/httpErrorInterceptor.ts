import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpInterceptor, HttpRequest, HttpEvent, HttpHandler, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { ErrorInfo } from '../shared/index';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(request).do((event: HttpEvent<any>) => {

        }, (err: any) => {
            if (err instanceof HttpErrorResponse) {
                let body = err.error || err;
                const errmsg: string = body.message || JSON.stringify(body);
                let errorInfo = new ErrorInfo().parseObservableResponseError(err.error);
                return Observable.throw(errorInfo);
            }
        });
    }

}
