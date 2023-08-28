import { Injectable } from '@angular/core';
import { Effect, Actions } from '@ngrx/effects';
import * as Users from './users.actions';
import { UsersService } from '../../services/crm.services';

@Injectable()
export class UserEffects {
	constructor(
		private action$: Actions,
		private svc: UsersService,
	) { }

	@Effect() loadUsers$ = this.action$
		.ofType(Users.UsersActionTypes.LOAD)
		.switchMap(() => this.svc.getUsers('', 100, 0, ''))
		.map(users => new Users.UsersLoaded(users));
}
