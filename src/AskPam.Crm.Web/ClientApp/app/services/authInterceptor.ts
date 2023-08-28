import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Const } from '../app.consts';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor() { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = localStorage.getItem(Const.idToken);
        const organizationId = localStorage.getItem(Const.organizationId);

        if (token) {
            return next.handle(req.clone({
                headers: req.headers.set('Authorization', 'Bearer ' + token)
                    .set('Organization', organizationId || '')
            }));
        }

        return next.handle(req);
    }
}
