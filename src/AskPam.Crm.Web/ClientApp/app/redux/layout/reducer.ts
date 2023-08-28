import { Action } from '@ngrx/store';
import * as layout from './actions';


export interface State {
	sidenavOpened: boolean;
	pageTitle: string;
}

const initialState: State = {
	sidenavOpened: false,
	pageTitle: "Ask PAM"
};

export function reducer(state = initialState, action: layout.Actions): State {
	switch (action.type) {
		case layout.CLOSE_SIDENAV:
			state.sidenavOpened = false;
			return Object.assign({}, state, { sidenavOpened: false });

		case layout.OPEN_SIDENAV:
			return Object.assign({}, state, { sidenavOpened: true });

		case layout.UPDATE_PAGE_TITLE: {
			return Object.assign({}, state, { pageTitle: action.payload });
		}
		default:
			return state;
	}
}

