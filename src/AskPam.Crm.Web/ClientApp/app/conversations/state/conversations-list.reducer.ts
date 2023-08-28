import { Action } from "@ngrx/store";
import { ConversationListDto, EnumValueDto } from "../../services/crm.services";
import { EntityState, createEntityAdapter } from "@ngrx/entity";
import * as fromConversaionsList from "./conversations-lists.actions";
import * as fromConversation from "../state/conversation.actions";

const adapter = createEntityAdapter<ConversationListDto>();

export interface State extends EntityState<ConversationListDto> {
  loaded: boolean;
  loading: boolean;
  hasNext: boolean;
  selectedConversationId: number;
}

export const initialState: State = adapter.getInitialState({
  loaded: false,
  loading: false,
  hasNext: false,
  selectedConversationId: null
});

export function ConversationsListReducer(
  state = initialState,
  action:
    | fromConversaionsList.ConverationsListActions
    | fromConversation.ConversationActions
): State {
  switch (action.type) {
    case fromConversaionsList.ConversationsListActionTypes.LOAD: {
      return {
        ...state,
        loading: true
      };
    }

    case fromConversaionsList.ConversationsListActionTypes.ADD_MANY: {
      let typedAction = action as fromConversaionsList.AddManyConverations;

      return adapter.addMany(typedAction.payload.items, {
        ...state,
        loaded: true,
        loading: false,
        hasNext: typedAction.payload.hasNext,
        selectedConversationId: state.selectedConversationId
      });
    }

    case fromConversaionsList.ConversationsListActionTypes.ADD_ALL: {
      let typedAction = action as fromConversaionsList.AddAllConverations;

      return adapter.addAll(typedAction.payload.items, {
        ...state,
        loaded: true,
        loading: false,
        hasNext: typedAction.payload.hasNext,
        selectedConversationId: state.selectedConversationId
      });
    }

    case fromConversaionsList.ConversationsListActionTypes.RESET: {
      return {
        ...initialState
      };
    }

    case fromConversation.ConversationActionTypes.ADD_MESSAGE: {
      let typedAction = action as fromConversation.AddMessage;
      let conversation = typedAction.payload;

      let conv = state.entities[typedAction.payload.id];

      // if the conversationExist, we need to update the last message. We have the build a new array with conversation replaced
      if (conv) {
        if (typedAction.payload.lastMessage.status !== "Sent") {
          typedAction.payload.seen = false;
        }

        state = adapter.removeOne(typedAction.payload.id, state);
      }
      return adapter.addOne(typedAction.payload, state);
    }

    case fromConversation.ConversationActionTypes.MARK_AS_READ: {
      let typedAction = action as fromConversation.MarkConversationAsRead;
      let conv = state.entities[typedAction.payload];
      if (conv) {
        conv.seen = true;
        return adapter.updateOne(
          { id: typedAction.payload, changes: conv },
          state
        );
      } else {
        return state;
      }
    }

    case fromConversation.ConversationActionTypes.MARK_AS_UNREAD: {
      let typedAction = action as fromConversation.MarkConversationAsUnread;
      let conv = state.entities[typedAction.payload];
      if (conv) {
        conv.seen = false;
        return adapter.updateOne(
          { id: typedAction.payload, changes: conv },
          state
        );
      } else {
        return state;
      }
    }

    case fromConversation.ConversationActionTypes.FLAG_SUCCESS: {
      let typedAction = action as fromConversation.FlagConversation;
      let entity = state.entities[typedAction.payload];
      if (entity) {
        return adapter.updateOne(
          {
            id: typedAction.payload,
            changes: { ...entity, isFlagged: !entity.isFlagged }
          },
          state
        );
      } else {
        return state;
      }
    }

    case fromConversation.ConversationActionTypes.STAR:
    case fromConversation.ConversationActionTypes.UNSTAR: {
      let typedAction = action as
        | fromConversation.StarConversation
        | fromConversation.UnStarConversation;
      let entity = state.entities[typedAction.payload.conversationId];
      if (entity) {
        return adapter.updateOne(
          {
            id: typedAction.payload.conversationId,
            changes: { ...entity, isStarred: !entity.isStarred }
          },
          state
        );
      } else {
        return state;
      }
    }

    case fromConversation.ConversationActionTypes.ARCHIVE: {
      let typedAction = action as fromConversation.ArchiveConversation;
      return adapter.removeOne(typedAction.payload, state);
    }

    case fromConversation.ConversationActionTypes.UPDATE_CONTACT: {
      let typedAction = action as fromConversation.UpdateConversationContact;
      let conv = state.entities[typedAction.payload.conversation.id];
      if (conv) {
        return adapter.updateOne(
          {
            id: typedAction.payload.conversation.id,
            changes: {
              ...conv,
              contact: typedAction.payload.contact
            }
          },
          state
        );
      } else {
        return state;
      }
    }

    case fromConversation.ConversationActionTypes.UNLINK_CHANNEL_CONTACT: {
      let typedAction = action as fromConversation.UnlinkConversationContact;
      let conv = state.entities[typedAction.payload.conversation.id];
      if (conv) {
        return adapter.updateOne(
          {
            id: typedAction.payload.conversation.id,
            changes: {
              ...conv,
              contact: null
            }
          },
          state
        );
      } else {
        return state;
      }
    }

    default: {
      return state;
    }
  }
}

export const getLoaded = (state: State) => state.loaded;
export const getHasNext = (state: State) => state.hasNext;
export const getLoading = (state: State) => state.loading;

export const getIds = (state: State) => state.ids;
export const getSelectedConversationId = (state: State) =>
  state.selectedConversationId;

export const {
  selectIds: getConversationIds,
  selectEntities: getConversationEntities,
  selectAll: getAllConversations,
  selectTotal: getTotalConversations
} = adapter.getSelectors();
