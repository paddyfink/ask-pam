import * as Contacts from './contacts.actions';
import { Action } from '@ngrx/store';
import { ContactGroupDto } from '../../services/crm.services';

export interface State {
	loaded: boolean;
	loading: boolean;
	items: ContactGroupDto[];
}

const initialState: State = {
	loaded: false,
	loading: false,
	items: []
};

export function reducer(state = initialState, action: Contacts.Actions): State {

	switch (action.type) {
		case Contacts.LOAD_CONTACT_GROUPS:
			{
				return {
					...state,
					loading: true
				};
			}
		case Contacts.LOAD_CONTACT_GROUPS_SUCCESS:
			{
				return <State>{
					loaded: true,
					loading: false,
					items: action.payload
				};
			}
		default:
			{
				return state;
			}
	}
}

export const getItems = (state: State) => state.items;
