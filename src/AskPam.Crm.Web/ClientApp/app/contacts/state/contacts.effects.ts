import { Injectable } from '@angular/core';
import { Effect, Actions } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import * as contacts from './contacts.actions';
import { ContactsService, PagedResultDtoOfContactListDto, ContactDto } from '../../services/crm.services';
import { ErrorInfo } from '../../shared/index';
import { SubmitFormSucess, SubmitFormError } from '../../redux/forms/forms.actions';

@Injectable()
export class ContactsEffects {
    constructor(
        private action$: Actions,
        private svc: ContactsService,
    ) { }

    @Effect() loadContacts$ = this.action$
        .ofType(contacts.LOAD_CONTACTS)
        .map((action: contacts.LoadContacts) => action.payload)
        .switchMap(request => this.svc.getContacts(request))
        .map((result) => new contacts.LoadContactsSuccess(result));

    @Effect() loadContactGroups$ = this.action$
        .ofType(contacts.LOAD_CONTACT_GROUPS)
        .map((action: contacts.LoadContactGroups) => action.payload)
        .switchMap(() => this.svc.getAllGroups())
        .map((result) => new contacts.LoadContactGroupsSuccess(result));

    @Effect() editContact$ = this.action$
        .ofType(contacts.EDIT_CONTACT)
        .map((action: contacts.EditContact) => action.payload)
        .switchMap((contact: ContactDto) => {
            if (contact.id !== 0) {
                return this.svc.updateContact(contact.id, contact).map(result =>
                    new SubmitFormSucess({ entity: 'contact', entityId: contact.id, result: result })
                )
                .catch(error => Observable.of(
                    new SubmitFormError({ entity: 'contact', entityId: contact.id, error }))
                );
            } else {
                return this.svc.createContact(contact).map(result =>
                    new SubmitFormSucess({ entity: 'contact', entityId: contact.id, result: result })
                )
                .catch(error => {
                    let err = new ErrorInfo().parseObservableResponseError(error.response);
                    return Observable.of(
                    new SubmitFormError({ entity: 'contact', entityId: contact.id, error: err }));
                }
                );
            }
        });
}