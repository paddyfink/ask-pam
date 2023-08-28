import * as notifications from "./notifications.actions";
import { Action } from '@ngrx/store';
import { NotificationDto } from "../../services/crm.services";

export interface State {
	loading: boolean,
	items: NotificationDto[]
	hasNext: boolean,
	unreadCount: number
}

export const initialState: State = {
	unreadCount: 0,
	items: [],
	hasNext: false,
	loading: false
};

export function reducer(state = initialState, action: notifications.Actions): State {

	switch (action.type) {
		case notifications.LOAD_NOTICIFATIONS:
			{
				return Object.assign({},
					state,
					{
						loading: true
					});
			}

		case notifications.LOAD_NOTIFICATIONS_SUCCESS:
			{
				return Object.assign({},
					state,
					{
						items: [...state.items, ...action.payload.items],
						hasNext: action.payload.hasNext,
						loading: false
					});
			}

		case notifications.GET_UNREAD_COUNT_SUCCESS:
			{
				return Object.assign({},
					state,
					{
						unreadCount: action.payload
					});
			}

		case notifications.ADD_NOTIFICATION:
			{
				return Object.assign({},
					state,
					{
						unreadCount: state.unreadCount + 1,
						items: [action.payload, ...state.items]
					}
				);
			}

		case notifications.MARK_ALL_NOTIFICATIONS_AS_SEEN:
			{
				return Object.assign({},
					state,
					{
						unreadCount: 0,
					}
				);
			}

		case notifications.MARK_NOTIFICATION_AS_READ:
			{
				var index = state.items.map(c => c.id).indexOf(action.payload.id);

				var cloneConv: NotificationDto = state.items[index];

				if (cloneConv) {
					cloneConv.read = true;
					return Object.assign({}, state, {
						items: [
							...state.items.slice(0, index),
							cloneConv,
							...state.items.slice(index + 1)
						]
					});
				}
				else {
					return state;
				}
			}

		default:
			{
				return state;
			}
	}
}