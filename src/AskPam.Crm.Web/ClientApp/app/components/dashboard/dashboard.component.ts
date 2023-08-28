import { Component, AfterViewInit, OnInit } from '@angular/core';

import { TdMediaService, TdLoadingService } from '@covalent/core';

import { DashboardService, LibraryService, LibraryItemListDto, ContactsService, ContactListDto } from '../../services/crm.services';
import { Helper } from '../../shared/helper';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  providers: [DashboardService, LibraryService, ContactsService]
})

export class DashboardComponent implements OnInit, AfterViewInit {

  conversationsAssignedCount: number;
  conversationsFlaggedCount: number;
  conversationsUnreadCount: number;
  conversationsFollowedCount: number;
  conversationsCount: number;
  messagesReceivedCount: number;
  messagesSentCount: number;
  messagesUnreadCount: number;
  libraryItems: LibraryItemListDto[] = [];
  contacts: ContactListDto[] = [];

  constructor(
    public media: TdMediaService,
    private _loadingService: TdLoadingService,
    private dashboardService: DashboardService,
    private libraryService: LibraryService,
    private contactsService: ContactsService,
    private helper: Helper
  ) {
  }

  loadConversationsAssignedCount(): void {
    this._loadingService.register('conversationsAssignedCount');
    this.dashboardService.getConversationsAssignedCount()
      .subscribe(result => {
        this.conversationsAssignedCount = result;
        this._loadingService.resolve('conversationsAssignedCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('conversationsAssignedCount');
      });
  }

  loadConversationsFlaggedCount(): void {
    this._loadingService.register('conversationsFlaggedCount');
    this.dashboardService.getConversationsFlaggedCount()
      .subscribe(result => {
        this.conversationsFlaggedCount = result;
        this._loadingService.resolve('conversationsFlaggedCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('conversationsFlaggedCount');
      });
  }

  loadConversationsUnreadCount(): void {
    this._loadingService.register('conversationsUnreadCount');
    this.dashboardService.getConversationsUnreadCount()
      .subscribe(result => {
        this.conversationsUnreadCount = result;
        this._loadingService.resolve('conversationsUnreadCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('conversationsUnreadCount');
      });
  }

  loadConversationsFollowedCount(): void {
    this._loadingService.register('conversationsFollowedCount');
    this.dashboardService.getConversationsFollowedCount()
      .subscribe(result => {
        this.conversationsFollowedCount = result;
        this._loadingService.resolve('conversationsFollowedCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('conversationsFollowedCount');
      });
  }

  loadConversationsCount(): void {
    this._loadingService.register('conversationsCount');
    this.dashboardService.getConversationsCount()
      .subscribe(result => {
        this.conversationsCount = result;
        this._loadingService.resolve('conversationsCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('conversationsCount');
      });
  }

  loadMessagesReceivedCount(): void {
    this._loadingService.register('messagesReceivedCount');
    this.dashboardService.getMessagesReceivedCount()
      .subscribe(result => {
        this.messagesReceivedCount = result;
        this._loadingService.resolve('messagesReceivedCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('messagesReceivedCount');
      });
  }

  loadMessagesSentCount(): void {
    this._loadingService.register('messagesSentCount');
    this.dashboardService.getMessagesSentCount()
      .subscribe(result => {
        this.messagesSentCount = result;
        this._loadingService.resolve('messagesSentCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('messagesSentCount');
      });
  }

  loadMessagesUnreadCount(): void {
    this._loadingService.register('messagesUnreadCount');
    this.dashboardService.getMessagesUnreadCount()
      .subscribe(result => {
        this.messagesUnreadCount = result;
        this._loadingService.resolve('messagesUnreadCount');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('messagesUnreadCount');
      });
  }

  loadLibraryItems(): void {
    this._loadingService.register('libraryItems');
    this.libraryService.getLibraryItems({
      sorting: 'createdAt DESC',
      maxResultCount: 5,
      skipCount: 0
    })
      .subscribe(result => {
        this.libraryItems = result.items;
        this._loadingService.resolve('libraryItems');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('libraryItems');
      });
  }

  loadContacts(): void {
    this._loadingService.register('contacts');
    this.contactsService.getContacts({ maxResultCount: 5, sorting: 'createdAt DESC', skipCount: 0 })
      .subscribe(result => {
        this.contacts = result.items;
        this._loadingService.resolve('contacts');
      }, error => {
        this.helper.displayError(error.message);
        this._loadingService.resolve('contacts');
      });
  }

  ngAfterViewInit(): void {
    // broadcast to all listener observables when loading the page
    this.media.broadcast();
  }

  ngOnInit(): void {
    this.loadConversationsAssignedCount();
    this.loadConversationsCount();
    this.loadConversationsFlaggedCount();
    this.loadConversationsFollowedCount();
    this.loadConversationsUnreadCount();
    this.loadMessagesReceivedCount();
    this.loadMessagesSentCount();
    this.loadMessagesUnreadCount();
    this.loadLibraryItems();
    this.loadContacts();
  }
}
