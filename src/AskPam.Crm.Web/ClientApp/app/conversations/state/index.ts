import * as fromConversationsFilters from './conversations-filters.reducer';
import * as fromConversationList from './conversations-list.reducer';
import * as fromConversation from './conversation.reducer';
import * as fromRoot from '../../redux';
import { createFeatureSelector, createSelector } from '@ngrx/store';



export interface ConversationsState extends fromRoot.AppState {
    filters: fromConversationsFilters.State;
    conversations: fromConversationList.State;
    conversation: fromConversation.State;
}


export const reducers = {
    filters: fromConversationsFilters.reducer,
    conversations: fromConversationList.ConversationsListReducer,
    conversation: fromConversation.reducer,
};

// State
export const getConversationsState = createFeatureSelector<ConversationsState>('conversations');

// Conversations
export const getConversationsListState = createSelector(getConversationsState, (state: ConversationsState) => state.conversations);
export const getConversationsList = createSelector(getConversationsListState, fromConversationList.getAllConversations);
export const getConversationsListLoading = createSelector(getConversationsListState, fromConversationList.getLoading);
export const getConversationsListHasNext = createSelector(getConversationsListState, fromConversationList.getHasNext);
export const getCurrentConversationId = createSelector(getConversationsListState, fromConversationList.getSelectedConversationId);


// Conversations Filters
export const getConversationsFilterState = createSelector(
    getConversationsState,
    (state: ConversationsState) => state.filters);
export const getConversationsFilters = createSelector(getConversationsFilterState, fromConversationsFilters.getConversationsFilters);
export const getConversationsFiltersLoaded = createSelector(getConversationsFilterState, fromConversationsFilters.getLoaded);
export const getCurrentConversationsFilter = createSelector(getConversationsFilterState, fromConversationsFilters.getCurrentFilter);

// Conversation
export const getConversationState = createSelector(getConversationsState, (state: ConversationsState) => state.conversation);
export const getConversation = createSelector(getConversationState, fromConversation.getConversation);
export const getConversationLoading = createSelector(getConversationState, fromConversation.getLoading);
export const getConversationError = createSelector(getConversationState, fromConversation.getError);


