import { Injectable } from '@angular/core';
import {
	Router,
	ActivatedRouteSnapshot,
	RouterStateSnapshot
} from '@angular/router';
import { CanActivate, CanActivateChild } from '@angular/router';
import { Auth } from '../../services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {

	constructor(private auth: Auth, private router: Router) { }

	canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		
		if (this.auth.authenticated()) {
			return true;
		} else {
			// Save URL to redirect to after login and fetching profile to get roles
			localStorage.setItem('redirect_url', state.url);
			this.router.navigate(['auth/login']);
			return false;
		}
	}

	canActivateChild(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

		return this.canActivate(next, state);
	}
}
