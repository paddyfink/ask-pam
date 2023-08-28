import { Injectable } from '@angular/core';
import { Effect, Actions } from '@ngrx/effects';
import { ActionReducer, Action } from '@ngrx/store';
import * as session from './actions';
import { AccountService } from '../../services/index';

@Injectable()
export class SessionEffects {
    constructor(
        private action$: Actions,
        private svc: AccountService,
    ) { }

    @Effect() loadConversations$ = this.action$
        .ofType(session.GET_SESSION_INFO)
        .switchMap(() => this.svc.getInfo())
        .map(result => new session.GetSessionSuccess(result));
}