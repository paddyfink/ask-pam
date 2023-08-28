import { Injectable } from '@angular/core';
import {
	Router,
	ActivatedRouteSnapshot,
	RouterStateSnapshot
} from '@angular/router';
import { CanActivate, CanActivateChild } from '@angular/router';
import { Auth } from '../../services/auth.service';

@Injectable()
export class HostGuard implements CanActivate, CanActivateChild {

	constructor(private auth: Auth, private router: Router) { }

	canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		return this.auth.isHost$();
	}

	canActivateChild(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

		return this.canActivate(next, state);
	}
}