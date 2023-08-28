import { Action } from '@ngrx/store';
import { type } from '../../shared/util';
import { PagedResultDtoOfContactListDto, ContactGroupDto, ContactDto, ContactListRequestDto } from '../../services/crm.services';


export const LOAD_CONTACTS = type('[Contact] Load');
export const LOAD_CONTACTS_SUCCESS = type('[Contact] Load Success');
export const LOAD_CONTACT_GROUPS = type('[Contact] Load groups');
export const LOAD_CONTACT_GROUPS_SUCCESS = type('[Contact] Load Groups Success');
export const EDIT_CONTACT = type('[Contact] Edit');
export const EDIT_CONTACT_SUCCESS = type('[Contact] Edit Success');
export const UPDATE_FILTER = type('[Contacts] Update Filter');
export const CLEAR_FILTER = type('[Contacts] Clear Filter');


export class LoadContacts implements Action {
    type = LOAD_CONTACTS;
    constructor(public payload: ContactListRequestDto) { }
}

export class EditContact implements Action {
    type = EDIT_CONTACT;
    constructor(public payload: ContactDto) { }
}

export class LoadContactsSuccess implements Action {
    type = LOAD_CONTACTS_SUCCESS;
    constructor(public payload: PagedResultDtoOfContactListDto) { }
}

export class LoadContactGroups implements Action {
    type = LOAD_CONTACT_GROUPS;
    constructor(public payload) { }
}

export class LoadContactGroupsSuccess implements Action {
    type = LOAD_CONTACT_GROUPS_SUCCESS;
    constructor(public payload: ContactGroupDto[]) { }
}

export class UpdateFilter implements Action {
    type = UPDATE_FILTER;
    constructor(public payload) { }
}

export class ClearFilter implements Action {
    type = CLEAR_FILTER;
    constructor(public payload) { }
}


export type Actions = LoadContacts
    | LoadContactsSuccess
    | EditContact
    | LoadContactGroups
    | LoadContactGroupsSuccess
    | UpdateFilter
    | ClearFilter;
