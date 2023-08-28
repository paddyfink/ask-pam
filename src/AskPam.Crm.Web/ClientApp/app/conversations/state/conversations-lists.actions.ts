import { type } from '../../shared/util';
import { ConversationListRequestDto, PagedResultDtoOfConversationListDto } from '../../services/crm.services';
import { Action } from '@ngrx/store';

export const ConversationsListActionTypes = {
    LOAD: type('[Conversations List] Load'),
    ADD_MANY: type('[Conversation List] Add many'),
    ADD_ALL: type('[Conversationss List] Add All'),
    RESET: type('[Conversations List] Reset ')
};


export class LoadConversations implements Action {
    type = ConversationsListActionTypes.LOAD;
    constructor(public payload: ConversationListRequestDto) { }
}

export class AddManyConverations implements Action {
    type = ConversationsListActionTypes.ADD_MANY;
    constructor(public payload: PagedResultDtoOfConversationListDto) { }
}

export class AddAllConverations implements Action {
    type = ConversationsListActionTypes.ADD_ALL;
    constructor(public payload: PagedResultDtoOfConversationListDto) { }
}

export class ResetConversationList implements Action {
    type = ConversationsListActionTypes.RESET;
}

export type ConverationsListActions =
    LoadConversations
    | AddManyConverations
    | AddAllConverations
    | ResetConversationList;
