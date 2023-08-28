import * as contacts from './contacts.actions';
import { Action } from '@ngrx/store';
import { ContactListDto } from '../../services/crm.services';

export interface State {
	loading: boolean;
	items: ContactListDto[];
	totalCount: number;
	filter: {
		userId: string;
		groupId: number;
		withConversation: boolean;
		sortBy: string;
		sortOrder: string;
		searchTerm: string;
	};
}

const initialState: State = {
	loading: false,
	items: [],
	totalCount: 0,
	filter: {
		groupId: null,
		userId: null,
		withConversation: null,
		sortBy: 'createdAt',
		sortOrder: 'DESC',
		searchTerm: null
	}
};

export function reducer(state = initialState, action: contacts.Actions): State {

	switch (action.type) {
		case contacts.LOAD_CONTACTS:
			{
				return {
					...state,
					loading: true
				};
			}
		case contacts.LOAD_CONTACTS_SUCCESS:
			{
				return {
					...state,
					loading: false,
					items: action.payload.items,
					totalCount: action.payload.totalCount
				};
			}
		case contacts.UPDATE_FILTER:
			{
				return {
					...state,
					filter: {
						...state.filter,
						...action.payload
					}
				};
			}

		case contacts.CLEAR_FILTER:
			{
				return {
					...state,
					filter: {
						groupId: null,
						userId: null,
						withConversation: null,
						sortBy: 'createdAt',
						sortOrder: 'DESC',
						searchTerm: null
					}
				};
			}
		default:
			{
				return state;
			}
	}
}

export const getItems = (state: State) => state.items;
export const getLoading = (state: State) => state.loading;
export const getTotalcount = (state: State) => state.totalCount;
export const getFilter = (state: State) => state.filter;
