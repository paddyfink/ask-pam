import * as fromRoot from '../../redux';
import * as formContacts from './contacts.reducer';
import * as fromContactGroups from './contact-groups.reducer';
import { createFeatureSelector, createSelector } from '@ngrx/store';

export interface ContactsState extends fromRoot.AppState {
    contacts: formContacts.State;
    groups: fromContactGroups.State;
}

export const reducers = {
    contacts: formContacts.reducer,
	groups: fromContactGroups.reducer,
};

// State
export const getContactsState = createFeatureSelector<ContactsState>('contacts');

// Contact
export const getContactsListState = createSelector(getContactsState, (state: ContactsState) => state.contacts);
export const getContactsList = createSelector(getContactsListState, formContacts.getItems);
export const getContactFilter = createSelector(getContactsListState, formContacts.getFilter);
export const getContactsCount = createSelector(getContactsListState, formContacts.getTotalcount);
export const getContactsLoading = createSelector(getContactsListState, formContacts.getLoading);

export const getContactGroupsState = createSelector(getContactsState, (state: ContactsState) => state.groups);
export const getContactGroups = createSelector(getContactGroupsState, fromContactGroups.getItems);
