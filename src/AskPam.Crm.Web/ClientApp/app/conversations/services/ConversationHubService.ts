import { Injectable, EventEmitter, NgZone } from "@angular/core";
import { HubConnection } from "@aspnet/signalr-client";

import { ConversationListDto, ProfileDto } from "../../services/crm.services";
import { Auth } from "../../services/auth.service";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";
import { AppState } from "../../redux";
import {
  AddMessage,
  MarkConversationAsRead,
  ArchiveConversation,
  FlagConversation,
  FlagConversationSuccess,
  ArchiveSuccess
} from "../../conversations/state/conversation.actions";
import * as fromConversationsState from "../../conversations/state";

@Injectable()
export class ConversationHubService {
  public connectionEstablished: EventEmitter<Boolean> = new EventEmitter();
  public currentUser: ProfileDto;
  currentFilter;
  private connection: HubConnection;

  constructor(
    private authService: Auth,
    private store: Store<fromConversationsState.ConversationsState>,
    private zone: NgZone
  ) {
    this.store.select(state => state.session.profile).subscribe(profile => {
      this.currentUser = profile;
    });
    this.store
      .select(fromConversationsState.getCurrentConversationsFilter)
      .subscribe(filter => {
        this.currentFilter = filter;
      });

    this.connection = new HubConnection("/conversation");

    this.registerOnServerEvents();

    this.startConnection();

    this.connection.onclose = e => {
      this.startConnection();
    };
  }

  private startConnection(): void {
    const organizationId = this.authService.getUserOrganization();

    this.connection
      .start()
      .then(() => {
        this.connection.send("JoinGroup", organizationId);
        this.connection.send("JoinGroup", this.currentUser.id);
      })
      .catch(err => {
        console.log("Error while establishing connection");
      });
  }

  private registerOnServerEvents(): void {
    this.connection.on("newMessage", (conversation: ConversationListDto) => {
      console.log("New message");

      if (
        this.currentFilter === "AssignedToMe" &&
        conversation.assignedToId !== this.currentUser.id
      ) {
        return;
      }

      if (this.currentFilter === "AssignedToMe" && !conversation.assignedToId) {
        return;
      }
      if (this.currentFilter === "Flagged" && !conversation.isFlagged) {
        return;
      }

      if (this.currentFilter === "Contacts" && conversation.contact === null) {
        return;
      }
      if (this.currentFilter === "Leads" && conversation.contact !== null) {
        return;
      }
      if (this.currentFilter === "Unread" && conversation.seen) {
        return;
      }

      this.zone.run(() => {
        this.store.dispatch(new AddMessage(conversation));
      });
    });

    this.connection.on(
      "conversationRead",
      (conversation: ConversationListDto, userId: string) => {
        this.store
          .select(s => s.session.profile)
          .take(1)
          .subscribe(currentuser => {
            if (currentuser && currentuser.id !== userId) {
              this.zone.run(() => {
                this.store.dispatch(
                  new MarkConversationAsRead(conversation.id)
                );
              });
            }
          });
      }
    );

    this.connection.on(
      "conversationArchived",
      (conversationId: number, userId: string) => {
        // console.log('conversation archived');
        this.store
          .select(s => s.session.profile)
          .take(1)
          .subscribe(currentuser => {
            if (currentuser && currentuser.id !== userId) {
              this.zone.run(() => {
                this.store.dispatch(new ArchiveSuccess(conversationId));
              });
            }
          });
      }
    );

    this.connection.on(
      "conversationFlagged",
      (conversationId: number, userId: string) => {
        // console.log('conversation archived');
        this.store
          .select(s => s.session.profile)
          .take(1)
          .subscribe(currentuser => {
            if (currentuser && currentuser.id !== userId) {
              this.zone.run(() => {
                this.store.dispatch(
                  new FlagConversationSuccess(conversationId)
                );
              });
            }
          });
      }
    );
  }
}
