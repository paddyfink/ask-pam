import {
  UpdateConversationContact,
  ConversationActionTypes,
  ConversationActions
} from "./conversation.actions";
import { Action } from "@ngrx/store";
import {
  ConversationDto,
  ConversationListDto
} from "../../services/crm.services";
import * as conversation from "./conversation.actions";

export interface State {
  loaded: boolean;
  loading: boolean;
  item: ConversationDto;
  error: any;
}

export const initialState: State = {
  loaded: false,
  loading: false,
  item: {
    messages: [],
    channels: []
  },
  error: {}
};

export function reducer(
  state = initialState,
  action: ConversationActions
): State {
  switch (action.type) {
    case ConversationActionTypes.LOAD: {
      return {
        ...state,
        loading: true
      };
    }
    case ConversationActionTypes.LOADED: {
      let typedAction = action as conversation.ConverationLoaded;
      return {
        loaded: true,
        loading: false,
        item: typedAction.payload,
        error: {}
      };
    }

    case ConversationActionTypes.RESET: {
      return initialState;
    }

    case ConversationActionTypes.ADD_MESSAGE: {
      let typedAction = action as conversation.AddMessage;
      let conv = typedAction.payload;
      if (conv.id !== state.item.id) {
        return state;
      }

      let index = state.item.messages
        .map(m => m.id)
        .indexOf(conv.lastMessage.id);

      if (index > -1) {
        return state;
      }

      return {
        ...state,
        item: {
          ...state.item,
          messages: [...state.item.messages, conv.lastMessage]
        }
      };
    }

    case ConversationActionTypes.SEND_MESSAGE:
      return {
        ...state,
        error: {}
      };

    case ConversationActionTypes.SEND_MESSAGE_SUCCESS: {
      let typedAction = action as conversation.SendMessageSuccess;
      const index = state.item.messages
        .map(m => m.id)
        .indexOf(typedAction.payload.id);

      if (index > -1) {
        return state;
      }
      return {
        ...state,
        item: {
          ...state.item,
          messages: [...state.item.messages, typedAction.payload]
        }
      };
    }

    case ConversationActionTypes.ASSIGN: {
      let typedAction = action as conversation.AssignConversation;
      return {
        ...state,
        item: {
          ...state.item,
          assignedToId: typedAction.payload.user
            ? typedAction.payload.user.id
            : null,
          assignedToFullName: typedAction.payload.user
            ? typedAction.payload.user.fullName
            : "",
          assignedToPicture: typedAction.payload.user
            ? typedAction.payload.user.picture
            : ""
        }
      };
    }

    case ConversationActionTypes.ARCHIVE_SUCCESS: {
      let typedAction = action as conversation.ArchiveSuccess;
      if (typedAction.payload === state.item.id) {
        return initialState;
      } else {
        return state;
      }
    }

    case ConversationActionTypes.UPDATE_CONTACT_SUCCESS: {
      let typedAction = action as conversation.UpdateConversationContactSuccess;

      if (typedAction.payload.conversationId === state.item.id) {
        return {
          ...state,
          item: {
            ...state.item,
            contact: typedAction.payload.contact
          }
        };
      } else {
        return state;
      }
    }

    case ConversationActionTypes.UNLINK_CHANNEL_CONTACT: {
      let typedAction = action as conversation.UnlinkConversationContact;
      if (typedAction.payload.conversation.id === state.item.id) {
        return {
          ...state,
          item: {
            ...state.item,
            contact: null
          }
        };
      } else {
        return state;
      }
    }

    case ConversationActionTypes.STAR:
    case ConversationActionTypes.UNSTAR: {
      let typedAction = action as
        | conversation.StarConversation
        | conversation.UnStarConversation;

      if (state.item.id !== typedAction.payload.conversationId) {
        return state;
      }
      return {
        ...state,
        item: {
          ...state.item,
          isStarred: !state.item.isStarred
        }
      };
    }

    case ConversationActionTypes.FLAG_SUCCESS:
    case ConversationActionTypes.FLAG_ERROR: {
      let typedAction = action as
        | conversation.FlagConversationSuccess
        | conversation.FlagConversationError;

      if (state.item.id !== typedAction.payload) {
        return state;
      }
      return {
        ...state,
        item: {
          ...state.item,
          isFlagged: !state.item.isFlagged
        }
      };
    }

    case ConversationActionTypes.TOGGLE_BOT_ACTIVATION: {
      return {
        ...state,
        item: {
          ...state.item,
          botDisabled: !state.item.botDisabled
        }
      };
    }

    default: {
      return state;
    }
  }
}

export const getLoaded = (state: State) => state.loaded;
export const getLoading = (state: State) => state.loading;
export const getConversation = (state: State) => state.item;
export const getError = (state: State) => state.error;
