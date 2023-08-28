
// import {storeLogger} from 'ngrx-store-logger';
import { combineReducers, compose, ActionReducerMap, ActionReducer, MetaReducer, createFeatureSelector, createSelector } from '@ngrx/store';
import { RouterStateUrl } from './../shared/util';
import * as fromRouter from '@ngrx/router-store';
import { storeFreeze } from 'ngrx-store-freeze';


import * as fromLayout from './layout';
import * as fromSession from './session';
import * as fromNotifications from './notifications/notifications.reducer';
import { FormsReducer, FormState } from './forms';
import { environment } from '../../environments/environment';

export interface AppState {
	// users: UsersState;
	forms: FormState;
	layout: fromLayout.State;
	session: fromSession.State;
	notifications: fromNotifications.State;
	//  routerReducer: fromRouter.RouterReducerState<RouterStateUrl	>;
}

export const reducers: ActionReducerMap<AppState> = {

   // users: UsersReducer,
	layout: fromLayout.reducer,
	forms: FormsReducer,
	session: fromSession.reducer,
	notifications: fromNotifications.reducer
	// routerReducer: fromRouter.routerReducer,
};

// console.log all actions
export function logger(reducer: ActionReducer<AppState>): ActionReducer<AppState> {
	return function (state: AppState, action: any): AppState {
		console.log('state', state);
		console.log('action', action);

		return reducer(state, action);
	};
}

export const metaReducers: MetaReducer<AppState>[] = [];
// !environment.production ? [] : [];


