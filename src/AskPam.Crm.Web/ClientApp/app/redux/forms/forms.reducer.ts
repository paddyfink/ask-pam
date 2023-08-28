import {merge, omit} from 'lodash';
import * as forms from './forms.actions';

export interface FormState {
	message;
	note;
	contact;
}

const initialState: FormState = {
	message: {},
	note: {},
	contact: {},
};

export function FormsReducer(state = initialState, action: forms.Actions): FormState {
	switch (action.type) {
		case forms.UPDATE: {
			return merge({}, state, {
				[action.payload.entity]: {
					[action.payload.entityId]: action.payload.form
				}
			});
		}

		//case forms.UPDATE_MESSAGE: {
		// 	return _.merge({}, state, {
		// 		conversation: {
		// 			[action.payload.entityId]: {
		// 				message: action.payload.form
		// 			}
		// 		}
		// 	});
		// }

		// case forms.UPDATE_NOTE: {
		// 	return _.merge({}, state, {
		// 		conversation: {
		// 			[action.payload.entityId]: {
		// 				note: action.payload.form
		// 			}
		// 		}
		// 	});
		// }
		case forms.FORM_SUBMIT_SUCCESS: {

			return Object.assign({}, state, {
				[action.payload.entity]: omit(state[action.payload.entity], [action.payload.entityId])
			});
		}

		default: {
			return state;
		}
	}
}
