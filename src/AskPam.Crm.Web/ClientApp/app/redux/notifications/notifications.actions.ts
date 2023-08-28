import { Action } from '@ngrx/store';
import { type } from '../../shared/util'
import { PagedResultDtoOfNotificationDto, GetNotificationsRequestDto } from '../../services/crm.services';


export const LOAD_NOTICIFATIONS = type('[Notifications] Load');
export const LOAD_NOTIFICATIONS_SUCCESS = type('[Notifications] Load Success');
export const GET_UNREAD_COUNT = type('[Notifications] Get UnreadCount');
export const GET_UNREAD_COUNT_SUCCESS = type('[Notifications] Get UnreadCount Success');
export const ADD_NOTIFICATION = type('[Notifications] Add');
export const MARK_ALL_NOTIFICATIONS_AS_SEEN = type('[Notifications] Mark all as seen');
export const MARK_NOTIFICATION_AS_READ = type('[Notifications] Mark as Read');


export class LoadNotifications implements Action {
    type = LOAD_NOTICIFATIONS;
    constructor(public payload : GetNotificationsRequestDto) { }
}

export class LoadNotificationsSuccess implements Action {
    type = LOAD_NOTIFICATIONS_SUCCESS;
    constructor(public payload: PagedResultDtoOfNotificationDto) { }
}

export class GetUnreadCount implements Action {
    type = GET_UNREAD_COUNT;
    constructor(public payload) { }
}

export class GetUnreadCountSuccess implements Action {
    type = GET_UNREAD_COUNT_SUCCESS;
    constructor(public payload) { }
}


export class AddNotification implements Action {
    type = ADD_NOTIFICATION;
    constructor(public payload) { }
}

export class MarkNotificationAsRead implements Action {
    type = MARK_NOTIFICATION_AS_READ;
    constructor(public payload: number) { }
}

export class MarkAllNotificationAsSeen implements Action {
    type = MARK_ALL_NOTIFICATIONS_AS_SEEN;
    constructor(public payload) { }
}



export type Actions = LoadNotifications
    | LoadNotificationsSuccess
    | AddNotification
    | MarkNotificationAsRead
    | MarkAllNotificationAsSeen
    | GetUnreadCount
    | GetUnreadCountSuccess