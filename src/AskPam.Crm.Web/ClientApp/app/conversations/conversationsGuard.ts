import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';

@Injectable()
export class ConverationsGuard implements CanActivate {

    constructor(private _router: Router) { }
    canActivate(route: ActivatedRouteSnapshot) {
        this._router.navigate(['/conversations', 'all', '']);
        return false;
    }
}
