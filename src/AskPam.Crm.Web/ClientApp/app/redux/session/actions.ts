import { Action } from '@ngrx/store';
import { type } from '../../shared/util'


export const GET_SESSION_INFO = type('[Session] Get Info');
export const GET_SESSION_INFO_SUCCESS = type('[Session] Get Info success');

export class GetSession implements Action {
	type = GET_SESSION_INFO;
	constructor(public payload) { }
}

export class GetSessionSuccess implements Action {
	type = GET_SESSION_INFO_SUCCESS;
	constructor(public payload) { }
}

export type Actions = GetSession | GetSessionSuccess