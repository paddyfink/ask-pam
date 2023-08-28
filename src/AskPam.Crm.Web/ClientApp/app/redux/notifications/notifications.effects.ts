import { Injectable } from '@angular/core';
import { Effect, Actions } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import * as notifications from './notifications.actions';
import { NotificationsService } from '../../services/crm.services';
import { of } from 'rxjs/observable/of';

@Injectable()
export class NotificationsEffects {
    constructor(
        private action$: Actions,
        private svc: NotificationsService,
    ) { }

    @Effect() loadNotifications$ = this.action$
        .ofType(notifications.LOAD_NOTICIFATIONS)
        .map((action: notifications.LoadNotifications) => action.payload)
        .switchMap(request => this.svc.getNotifications(request))
        .map((result) => new notifications.LoadNotificationsSuccess(result));

    @Effect() getUnreadCount$ = this.action$
        .ofType(notifications.GET_UNREAD_COUNT)
        .map((action: notifications.GetUnreadCount) => action.payload)
        .switchMap(request => this.svc.getUnreadNotificationsCount())
        .map((result) => new notifications.GetUnreadCountSuccess(result));

    @Effect() readNotification$ = this.action$
        .ofType(notifications.MARK_NOTIFICATION_AS_READ)
        .map((action: notifications.MarkNotificationAsRead) => action.payload)
        .switchMap(notificationId => this.svc.read(notificationId));

    @Effect() markAllAsSeen$ = this.action$
        .ofType(notifications.MARK_ALL_NOTIFICATIONS_AS_SEEN)
        .map((action: notifications.MarkAllNotificationAsSeen) => action.payload)
        .switchMap(() => this.svc.markAllNotificationAsSeen()
        .switchMap(() => of())
        )
}