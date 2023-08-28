import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InitialsPipe } from './pipes/initials.pipe';
import { NotificationLinkPipe } from './pipes/notificationLink.pipe';
import { OrderByPipe } from './pipes/orderBy.pipe';
import { KeysPipe } from './pipes/keys.pipe';

import { AutosizeDirective } from './directives/textarea-autosize';
import { ConnectFormDirective } from './directives/connectForm.directive';
import { EqualValidator } from './validators/equal-validator.directive';
import { ChannelIconComponent } from './components/channel-icon.component';
import { ActionConfirmDialogComponent } from './action-confirm-dialog/action-confirm-dialog.component';
import { TagsModule } from '../tags/tags.module';

import {
  MatButtonModule, MatCardModule, MatIconModule,
  MatListModule, MatMenuModule, MatTooltipModule,
  MatSlideToggleModule, MatInputModule, MatCheckboxModule,
  MatToolbarModule, MatSnackBarModule, MatSidenavModule,
  MatTabsModule, MatSelectModule, MatProgressBarModule, MatChipsModule, MatLineModule,
   MatFormFieldModule, MatRadioModule, MatRippleModule, MatPaginatorModule,
  MatButtonToggleModule, MatAutocompleteModule, MatSliderModule, MatDialogModule, MatTableModule
} from '@angular/material';

import {
  CovalentDataTableModule, CovalentMediaModule, CovalentLoadingModule,
   CovalentPagingModule, CovalentSearchModule,   CovalentCommonModule,
   CovalentLayoutModule,  CovalentNotificationsModule, CovalentDialogsModule,
   CovalentMessageModule,   CovalentExpansionPanelModule, CovalentChipsModule,
    CovalentFileModule, CovalentMenuModule
} from '@covalent/core';

import { ContactCardComponent } from './components/contact-card/contact-card.component';
import { ContactListComponent } from './components/contact-list/contact-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ContactListDialogComponent } from './components/contact-list-dialog/contact-list-dialog.component';
import { CreateContactDialogComponent } from './components/create-contact-dialog/create-contact-dialog.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { LibraryListDialogComponent } from './components/library-list-dialog/library-list.dialog.component';
import { ConversationListComponent } from './components/conversation-list/conversation-list.component';
import { ComposeMessageDialogComponent } from './components/compose-message-dialog/compose-message-dialog.component';
import { TagInputModule } from 'ngx-chips';
import { ComposeMessageToolbarComponent } from './components/compose-message-toolbar/compose-message-toolbar.component';
import {NgPipesModule} from 'ngx-pipes';

const MATERIAL_MODULES: any[] = [
  MatButtonModule, MatCardModule, MatIconModule,
  MatListModule, MatMenuModule, MatTooltipModule,
  MatSlideToggleModule, MatInputModule, MatCheckboxModule,
  MatToolbarModule, MatSnackBarModule, MatSidenavModule,
  MatTabsModule, MatSelectModule, MatProgressBarModule,
  MatProgressBarModule, MatChipsModule, MatLineModule, MatFormFieldModule, MatRadioModule, MatRippleModule, MatPaginatorModule,
  MatButtonToggleModule, MatAutocompleteModule, MatSliderModule, MatDialogModule, MatTableModule
];


@NgModule({
  entryComponents: [
    ActionConfirmDialogComponent,
    ContactListDialogComponent,
    CreateContactDialogComponent,
    LibraryListDialogComponent,
    ComposeMessageDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MATERIAL_MODULES,
    CovalentPagingModule,
    CovalentDataTableModule,
    CovalentSearchModule,
    ReactiveFormsModule,
    CovalentMessageModule,
    CovalentLoadingModule,
    CovalentMenuModule,
    CovalentCommonModule,
    CovalentLayoutModule,
    CovalentNotificationsModule,
    TagsModule,
    TagInputModule,
    CovalentChipsModule,
    CovalentFileModule,
    NgPipesModule

  ],
  declarations: [
    OrderByPipe,
    InitialsPipe,
    NotificationLinkPipe,
    KeysPipe,

    // Directives
    AutosizeDirective,
    ConnectFormDirective,

    // Validators todo: check this
    EqualValidator,

    // components
    ChannelIconComponent,
    ActionConfirmDialogComponent,
    ContactCardComponent,
    ContactListComponent,
    ContactListDialogComponent,
    CreateContactDialogComponent,
    ToolbarComponent,
    LibraryListDialogComponent,
    ConversationListComponent,
    ComposeMessageDialogComponent,
    ComposeMessageToolbarComponent

  ],
  providers: [],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MATERIAL_MODULES,

    // Covalent
    CovalentDataTableModule,
    CovalentMediaModule,
    CovalentLoadingModule,
    CovalentPagingModule,
    CovalentSearchModule,
    CovalentCommonModule,
    CovalentLayoutModule,
    CovalentNotificationsModule,
    CovalentDialogsModule,
    CovalentMessageModule,
    CovalentExpansionPanelModule,
    CovalentChipsModule,
    CovalentFileModule,
    CovalentMenuModule,
    NgPipesModule,

    OrderByPipe,
    InitialsPipe,
    NotificationLinkPipe,
    KeysPipe,
    AutosizeDirective,
    ConnectFormDirective,

    ChannelIconComponent,
    ContactCardComponent,
    ContactListComponent,
    ContactListDialogComponent,
    CreateContactDialogComponent,
    ToolbarComponent,
    ConversationListComponent,
    ComposeMessageToolbarComponent
  ]
})
export class SharedModule { }
