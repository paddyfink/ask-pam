import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { TagsModule } from '../tags/tags.module';
import { InternalNotesModule } from '../internal-notes/internal-notes.module';
import { FollowersModule } from '../followers/followers.module'


import { TagInputModule } from 'ngx-chips';
import { CKEditorModule } from 'ng2-ckeditor';
import { LinkyModule } from 'angular-linky';

import { LibraryPageComponent } from './library-page/library-page.component';
import { LibraryDetailPageComponent } from './library-detail-page/library-detail-page.component';
import { LibraryEditPageComponent } from './library-edit-page/library-edit-page.component';
import { routing } from './library.routing';


import { LibraryService, ContactsService ,ConversationsService} from '../services/crm.services';

@NgModule({
    declarations: [
        LibraryPageComponent,
        LibraryDetailPageComponent,
        LibraryEditPageComponent
    ],
    entryComponents: [
    ],
    imports: [
        RouterModule,
        routing,

        SharedModule,
        InternalNotesModule,
        TagsModule,
        CKEditorModule,
        LinkyModule,
        FollowersModule,
        TagInputModule],
    exports: [],
    providers: [LibraryService, ContactsService, ConversationsService],
})
export class LibraryModule { }