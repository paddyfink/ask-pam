import { type } from '../../shared/util';
import { Action } from '@ngrx/store';
import { EnumValueDto } from '../../services/crm.services';

export const ConversationsFiltersActionTypes = {
    LOAD: type('[Conversations Filters] Load '),
    LOADED: type('[Conversations Filters] Loaded'),
    SELECT: type('[Conversations Filters] Select ')
};

export class LoadConversationFilters implements Action {
    type = ConversationsFiltersActionTypes.LOAD;
    constructor(public payload: {}) { }
}

export class LoadConversationFiltersSuccess implements Action {
    type = ConversationsFiltersActionTypes.LOADED;
    constructor(public payload: EnumValueDto[]) { }
}

export class SelectConversationFilter implements Action {
    type = ConversationsFiltersActionTypes.SELECT;
    constructor(public payload: string) { }
}

export type ConversationsFiltersActions = LoadConversationFilters
    | LoadConversationFiltersSuccess
    | SelectConversationFilter;
