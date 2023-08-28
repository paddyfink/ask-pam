import { Injectable } from "@angular/core";
import { Effect, Actions } from "@ngrx/effects";
import { ActionReducer, Action } from "@ngrx/store";
import "rxjs/add/operator/switchMap";
import "rxjs/add/operator/map";
import { Observable } from "rxjs/Observable";
import * as fromConversation from "./conversation.actions";
import * as fromConversationsList from "./conversations-lists.actions";
import { ConversationsService } from "../../services/crm.services";
import {
  SubmitFormError,
  SubmitFormSucess
} from "./../../redux/forms/forms.actions";
import { of } from "rxjs/observable/of";
import { ActionConfirmDialogComponent } from "../../shared/action-confirm-dialog/action-confirm-dialog.component";
import { MatDialog } from "@angular/material";
import { empty } from "rxjs/observable/empty";
import { InAppNotificationService } from "../../shared/inAppNotification.service";
import {
  ConversationsFiltersActionTypes,
  LoadConversationFiltersSuccess
} from "./conversations-filters.action";
import { ConversationsListActionTypes } from "./conversations-lists.actions";
import { ConversationActionTypes } from "./conversation.actions";
import { ErrorInfo } from "../../shared";

@Injectable()
export class ConversationEffects {
  constructor(
    private action$: Actions,
    private svc: ConversationsService,
    private dialog: MatDialog,
    private notificationService: InAppNotificationService
  ) {}

  @Effect()
  loadConversations$ = this.action$
    .ofType(ConversationsListActionTypes.LOAD)
    .map((action: fromConversationsList.LoadConversations) => action.payload)
    .switchMap(request => this.svc.getConversations(request))
    .map(result => new fromConversationsList.AddManyConverations(result));

  @Effect()
  loadConversationFilters$ = this.action$
    .ofType(ConversationsFiltersActionTypes.LOAD)
    .switchMap(() => this.svc.getFilters())
    .map(filters => new LoadConversationFiltersSuccess(filters));

  @Effect()
  getConversation$ = this.action$
    .ofType(ConversationActionTypes.LOAD)
    .map((action: fromConversation.LoadConversation) => action.payload)
    .switchMap(id => this.svc.getConversation(id))
    .map(conversation => new fromConversation.ConverationLoaded(conversation));

  @Effect()
  flagConversation$ = this.action$
    .ofType(ConversationActionTypes.FLAG)
    .switchMap((action: fromConversation.FlagConversation) =>
      this.svc
        .flag(action.payload)
        .map(
          result => new fromConversation.FlagConversationSuccess(action.payload)
        )
        .catch(e => {
          this.notificationService.ApiError(e);
          return of(new fromConversation.FlagConversationError(action.payload));
        })
    );

  @Effect()
  starConversation$ = this.action$
    .ofType(ConversationActionTypes.STAR)
    .switchMap((action: fromConversation.StarConversation) =>
      this.svc
        .star(action.payload.conversationId, action.payload.isStarred)
        .switchMap(() => of()) // do nothing when it succeeds
        .catch(e => {
          this.notificationService.ApiError(e);
          return of(
            new fromConversation.UnStarConversation({
              conversationId: action.payload.conversationId
            })
          ); // dispatch an unrate action
        })
    );

  @Effect()
  archiveConversation$ = this.action$
    .ofType(ConversationActionTypes.ARCHIVE)
    .switchMap((action: fromConversation.ArchiveConversation) =>
      this.svc
        .archive(action.payload)
        .map(result => new fromConversation.ArchiveSuccess(action.payload))
        .catch(e => {
          this.notificationService.ApiError(e);
          return of(new fromConversation.ArchiveError(action.payload)); // dispatch an unrate action
        })
    );

  @Effect()
  assignConversation$ = this.action$
    .ofType(ConversationActionTypes.ASSIGN)
    .map((action: fromConversation.AssignConversation) => action.payload)
    .switchMap(request =>
      this.svc
        .assignToUser(
          request.conversationId,
          request.user ? request.user.id : ""
        )
        .switchMap(() => of())
    );

  @Effect()
  sendMessage$ = this.action$
    .ofType(ConversationActionTypes.SEND_MESSAGE)
    .map((action: fromConversation.SendMessage) => action.payload)
    .switchMap(message =>
      this.svc
        .sendMessage(message)
        .mergeMap(newMessage => {
          return [
            new fromConversation.SendMessageSuccess(newMessage),
            new SubmitFormSucess({
              entity: newMessage.type === "Note" ? "note" : "message",
              entityId: message.conversationId
            })
          ];
        })
        .catch(error => {
          const errorInfo = new ErrorInfo().parseObservableResponseError(
            error.response
          );
          return Observable.of(
            new SubmitFormError({
              entity: "message",
              entityId: message.conversationId,
              error: errorInfo
            })
          );
        })
    );

  @Effect()
  flagConversationConfirmDialogOpen: Observable<Action> = this.action$
    .ofType(ConversationActionTypes.FLAG_CONFIRM_DIALOG_OPEN)
    .map(
      (action: fromConversation.FlagConversationConfirmDialogOpen) =>
        action.payload
    )
    .switchMap(payload => {
      this.dialog.open(ActionConfirmDialogComponent, {
        data: {
          action: payload.action,
          text: payload.isFlagged
            ? ""
            : "Flag status is visible by everyone. Admins and people following the conversation will be notified. Continue?",
          title: payload.isFlagged ? "Remove flag status" : "Flag conversation"
        }
      });
      return empty();
    });

  @Effect()
  linkConversation$ = this.action$
    .ofType(ConversationActionTypes.UPDATE_CONTACT)
    .switchMap((action: fromConversation.UpdateConversationContact) =>
      this.svc
        .linkContactToConversation(
          action.payload.conversation.id,
          action.payload.contact.id
        )
        .map(
          contact =>
            new fromConversation.UpdateConversationContactSuccess({
              conversationId: action.payload.conversation.id,
              contact: contact
            })
        )
        .catch(e => {
          this.notificationService.ApiError(e);
          return empty();
          // return of(new fromConversation.UnlinkConversationContact({ conversation: action.payload.conversation }));
        })
    );

  @Effect()
  unlinkConveration$ = this.action$
    .ofType(ConversationActionTypes.UNLINK_CHANNEL_CONTACT)
    .switchMap(
      (action: fromConversation.UnlinkConversationContact) =>
        this.svc
          .unlinkContactToConversation(action.payload.conversation.id)
          .switchMap(() => of()) // do nothing when it succeeds
    );

  @Effect()
  toggleBot$ = this.action$
    .ofType(ConversationActionTypes.TOGGLE_BOT_ACTIVATION)
    .switchMap(
      (action: fromConversation.ToggleBotActivation) =>
        this.svc.toogleBotActivation(action.payload).switchMap(() => of()) // do nothing when it succeeds
    );

  @Effect()
  markAsUnread$ = this.action$
    .ofType(ConversationActionTypes.MARK_AS_UNREAD)
    .switchMap(
      (action: fromConversation.MarkConversationAsUnread) =>
        this.svc.markAsUnread(action.payload).switchMap(() => of()) // do nothing when it succeeds
    );
}
