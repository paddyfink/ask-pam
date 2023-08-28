import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { TagsModule } from '../tags/tags.module';
import { InternalNotesModule } from '../internal-notes/internal-notes.module';
import { FollowersModule } from '../followers/followers.module';


import { TagInputModule } from 'ngx-chips';

import { ContactDetailPageComponent } from './components/contact-detail-page/contact-detail-page.component';
import { ContactEditPageComponent } from './components/contact-edit-page/contact-edit-page.component';
import { ContactFilterComponent } from './components/contact-filter/contact-filter.component';
import { ContactsPageComponent } from './components/contacts-page/contacts-page.component';
import { routing } from './contacts.routing';

import { ContactsService } from '../services/crm.services';
import { reducers } from './state';
import { StoreModule } from '@ngrx/store';

@NgModule({
    declarations: [
        ContactsPageComponent,
        ContactDetailPageComponent,
        ContactEditPageComponent,
        ContactFilterComponent,
    ],
    entryComponents: [],
    imports: [
        RouterModule,
        routing,
        StoreModule.forFeature('contacts', reducers),
        SharedModule,
        InternalNotesModule,
        TagsModule,
        FollowersModule,
        TagInputModule],
    exports: [],
    providers: [ContactsService],
})
export class ContactsModule { }
