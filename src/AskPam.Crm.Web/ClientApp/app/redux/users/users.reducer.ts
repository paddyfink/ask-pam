import { Action, createSelector } from '@ngrx/store';
import { UserDto } from '../../services/crm.services';
import * as fromUsers from './users.actions';
import { createEntityAdapter, EntityState } from '@ngrx/entity';

export const adapter = createEntityAdapter<UserDto>();

export interface State extends EntityState<UserDto> {
	loaded: boolean;
	loading: boolean;
	hasNext: boolean;
	totalCount: number;
}


export const initialState: State = adapter.getInitialState({
	loaded: false,
	loading: false,
	hasNext: false,
	totalCount: 0
});

export function reducer(state = initialState, action: fromUsers.Actions): State {
	switch (action.type) {
		case fromUsers.UsersActionTypes.LOAD: {
			return {
				...state,
				loading: true
			};
		}

		case fromUsers.UsersActionTypes.LOADED: {
			let typedAction = action as fromUsers.UsersLoaded;

			return adapter.addAll(typedAction.payload.items, {
				...state,
				loaded: true,
				loading: false,
				hasNext: typedAction.payload.hasNext,
				totalCount: state.totalCount,
			});
		}

		case fromUsers.UsersActionTypes.ADD: {
			let typedAction = action as fromUsers.AddUser;
			return adapter.addOne(typedAction.payload, state);
		}

		case fromUsers.UsersActionTypes.UPDATE: {
			let typedAction = action as fromUsers.UpdateUser;
			return adapter.updateOne({ id: typedAction.payload.id, changes: typedAction.payload }, state);
		}

		default: {
			return state;
		}
	}
}


export const getLoaded = (state: State) => state.loaded;
export const getHasNext = (state: State) => state.hasNext;
export const getLoading = (state: State) => state.loading;



