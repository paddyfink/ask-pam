import { Action } from '@ngrx/store';
import { type } from '../../shared/util'


export const OPEN_SIDENAV = type('[Layout] Open Sidenav');
export const CLOSE_SIDENAV = type('[Layout] Close Sidenav');
export const UPDATE_PAGE_TITLE = type('[Layout] Update Page Title');

export class UpdatePageTitle implements Action {
	type = UPDATE_PAGE_TITLE;
	constructor(public payload: string) { }
}


export class OpenSidenav implements Action {
	type = OPEN_SIDENAV;
	constructor(public payload) { }
}


export class CloseSidenav implements Action {
	type = CLOSE_SIDENAV;
	constructor(public payload) { }
}

export type Actions = UpdatePageTitle
	| CloseSidenav
	| OpenSidenav