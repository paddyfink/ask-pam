import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { TagsModule } from '../tags/tags.module';
import { InternalNotesModule } from '../internal-notes/internal-notes.module';
import { FollowersModule } from '../followers/followers.module';


import { TagInputModule } from 'ngx-chips';
import { CKEditorModule } from 'ng2-ckeditor';
import { LinkyModule } from 'angular-linky';
import { LibraryService, ContactsService, ConversationsService } from '../services/crm.services';
import { ConverationsGuard } from './conversationsGuard';
import { reducers } from './state';
import { StoreModule } from '@ngrx/store';
import { ConversationsComponent } from './components/conversations/conversations.component';
import { ConversationDetailComponent } from './components/conversation-detail/conversation-detail.component';
import { EmailDetailsComponent } from './components/email-detail/email-details.component';
import { ConversationSidenavComponent } from './components/conversation-sidenav/conversation-sidenav.component';
import { ConversationsFilterComponent } from './components/conversations-filter/conversations-filter.component';
import { ConversationMessageComponent } from './components/conversation-message/conversation-message.component';
import { ComposeMessageComponent } from './components/compose-message/compose-message.component';
import { MassMessagingComponent } from './components/mass-messaging/mass-messaging.component';
import { routing } from './conversations.routing';
import { MessageTagComponent } from './components/conversation-message/message-tag.component';
import {AgmCoreModule} from '@agm/core';
@NgModule({
    declarations: [
        ConversationsComponent,
        ConversationDetailComponent,
        EmailDetailsComponent,
        ConversationSidenavComponent,
        ConversationsFilterComponent,
        ConversationMessageComponent,
        ComposeMessageComponent,
        MassMessagingComponent,
        MessageTagComponent
    ],
    entryComponents: [MessageTagComponent],
    imports: [
        RouterModule,
        routing,
        StoreModule.forFeature('conversations', reducers),
        AgmCoreModule,
        SharedModule,
        InternalNotesModule,
        TagsModule,
        CKEditorModule,
        LinkyModule,
        FollowersModule,
        TagInputModule],
    exports: [],
    providers: [ConverationsGuard, LibraryService, ContactsService, ConversationsService],
})
export class ConversationsModule { }
