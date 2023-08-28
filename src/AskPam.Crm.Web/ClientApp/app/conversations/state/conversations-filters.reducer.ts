import { EnumValueDto } from '../../services/crm.services';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import {
    ConversationsFiltersActions, ConversationsFiltersActionTypes,
    LoadConversationFiltersSuccess, SelectConversationFilter
} from './conversations-filters.action';

const adapter = createEntityAdapter<EnumValueDto>();

export interface State extends EntityState<EnumValueDto> {
    loaded: boolean;
    currentFilter: string;
}

const initialState: State = adapter.getInitialState({
    loaded: false,
    currentFilter: 'All',
});

export function reducer(state = initialState, action: ConversationsFiltersActions): State {
    switch (action.type) {
        case ConversationsFiltersActionTypes.LOADED: {
            let typedAction = action as LoadConversationFiltersSuccess;

            return adapter.addMany(typedAction.payload, {
                ...state,
                currentFilter: state.currentFilter,
                loaded: true,
            });
        }

        case ConversationsFiltersActionTypes.SELECT: {
            let typedAction = action as SelectConversationFilter;
            return {
                ...state,
                currentFilter: typedAction.payload
            };
        }

        default: {
            return state;
        }
    }
}

export const getLoaded = (state: State) => state.loaded;
export const getCurrentFilter = (state: State) => state.currentFilter;
export const getIds = (state: State) => state.ids;


export const {
    selectIds: getConversationsFiltersIds,
    // selectEntities: getConversationsFilters,
    selectAll: getConversationsFilters,
    // selectTotal: getTotalConversations,
} = adapter.getSelectors();
