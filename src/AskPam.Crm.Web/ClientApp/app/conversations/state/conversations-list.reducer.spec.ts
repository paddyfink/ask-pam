import * as fromConversationActions from './conversation.actions';
import * as fromConversationList from './conversations-list.reducer';
import { find } from 'lodash';
import * as fromConversationListActions from './conversations-lists.actions';
import { ConversationActionTypes } from './conversation.actions';
import { ConversationListDto, MessageDto, ContactListDto } from '../../services/crm.services';

const conversations: ConversationListDto[] = [{
    id: 1,
    lastMessage: null,
    contact: null,
    seen: false,
    organizationId: 'id',
    isFlagged: false,
    isStarred: false,
    date: new Date()
},
{
    id: 2,
    lastMessage: null,
    contact: null,
    seen: true,
    organizationId: 'id',
    isFlagged: true,
    isStarred: true,
    date: new Date()
}];
const entities = {
    1: conversations[0],
    2: conversations[1]
};
let state: fromConversationList.State;

describe('ConversationsListReducer', () => {

    beforeEach(() => {
        state = {
            entities: entities,
            loaded: true,
            loading: false,
            hasNext: false,
            selectedConversationId: null,
            ids: [1, 2]
        };
    });

    describe('LOAD', () => {

        it('should set loading to true', () => {
            const { initialState } = fromConversationList;
            const action = new fromConversationListActions.LoadConversations({ skipCount: 0, maxResultCount: 1, filter: '' });
            const actual = fromConversationList.ConversationsListReducer(initialState, action);
            expect({ ...actual }).toEqual({ ...initialState, loading: true });
        });
    });

    describe('ADD_MANY', () => {

        it('should populate entities from the array', () => {
            const { initialState } = fromConversationList;
            const action = new fromConversationListActions.AddManyConverations({ hasNext: true, items: conversations, totalCount: 2 });
            const actual = fromConversationList.ConversationsListReducer(initialState, action);
            expect(actual.loaded).toBeTruthy();
            expect(actual.loading).toBeFalsy();
            expect(actual.hasNext).toBeTruthy();
            expect(actual.entities).toEqual(entities);
        });

        it('should append entities from the array', () => {
            const newConv = {
                id: 3,
                lastMessage: null,
                contact: null,
                seen: true,
                organizationId: 'id',
                isFlagged: true,
                isStarred: true,
                date: new Date()
            };

            const action = new fromConversationListActions.AddManyConverations({ hasNext: true, items: [newConv], totalCount: 1 });
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual.loaded).toBeTruthy();
            expect(actual.loading).toBeFalsy();
            expect(actual.entities).toEqual({
                1: conversations[0],
                2: conversations[1],
                3: newConv
            });
        });

    });

    describe('ADD_ALL', () => {

        it('should populate entities from the array', () => {
            const { initialState } = fromConversationList;
            const action = new fromConversationListActions.AddAllConverations({ hasNext: true, items: conversations, totalCount: 2 });
            const actual = fromConversationList.ConversationsListReducer(initialState, action);
            expect(actual.loaded).toBeTruthy();
            expect(actual.loading).toBeFalsy();
            expect(actual.hasNext).toBeTruthy();
            expect(actual.entities).toEqual(entities);
        });

        it('should replace entities in the state', () => {

            const newConv = {
                id: 3,
                lastMessage: null,
                contact: null,
                seen: true,
                organizationId: 'id',
                isFlagged: true,
                isStarred: true,
                date: new Date()
            };

            const action = new fromConversationListActions.AddAllConverations({ hasNext: true, items: [newConv], totalCount: 1 });
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual.loaded).toBeTruthy();
            expect(actual.loading).toBeFalsy();
            expect(actual.hasNext).toBeTruthy();
            expect(actual.entities).toEqual({
                3: newConv
            });
        });

    });

    describe('RESET', () => {
        it('should restore initial sate', () => {
            const { initialState } = fromConversationList;
            const action = new fromConversationListActions.ResetConversationList();
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual).toEqual(initialState);
        });
    });

    describe('ARCHIVE', () => {
        it('should remove conversation', () => {
            const action = new fromConversationActions.ArchiveConversation(2);
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual.entities).toEqual({ 1: conversations[0] });
        });

        it('should return current state', () => {
            const action = new fromConversationActions.ArchiveConversation(3);
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual).toEqual(state);
        });
    });

    describe('ADD_MESSAGE', () => {

        it('should update existing conversation', () => {

            const newmessage = <MessageDto>{
                text: 'new',
                date: new Date(),
                author: 'John Doe'
            };

            const action = new fromConversationActions.AddMessage({ ...conversations[1], lastMessage: newmessage });
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual).toEqual({
                ...state,
                entities: {
                    1: conversations[0],
                    2: { ...conversations[1], lastMessage: newmessage, seen: false }
                }
            });
        });

        it('should add new conversation', () => {

            const newConv = {
                id: 3,
                lastMessage: {
                    id: 1, conversationId: 3, seen: false, attachmentsCount: 0, text: 'new', date: new Date(), author: 'John Doe'
                },
                contact: null,
                seen: true,
                organizationId: 'id',
                isFlagged: true,
                isStarred: true,
                date: new Date()
            };

            const action = new fromConversationActions.AddMessage(newConv);
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual).toEqual({
                ...state,
                entities: {
                    1: conversations[0],
                    2: conversations[1],
                    3: newConv,
                },
                ids: [1, 2, 3]
            });
        });
    });

    describe('MARK_AS_READ', () => {

        it('should update conversation', () => {
            let conv = conversations[0];
            expect(conv.seen).toEqual(false);
            const action = new fromConversationActions.MarkConversationAsRead(1);
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState.entities[1].seen).toEqual(true);
        });
    });

    describe('MARK_AS_UNREAD', () => {

        it('should update conversation', () => {
            let conv = conversations[1];
            expect(conv.seen).toEqual(true);
            const action = new fromConversationActions.MarkConversationAsUnread(2);
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState.entities[2].seen).toEqual(false);
        });
    });

    describe('FLAG|UNFLAG', () => {

        it('should flag', () => {
            let conv = conversations[0];
            expect(conv.isFlagged).toEqual(false);
            const action = new fromConversationActions.FlagConversation(1);
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState.entities[1].isFlagged).toEqual(true);
        });

        it('should unflag', () => {
            let conv = conversations[1];
            expect(conv.isFlagged).toEqual(true);
            const action = new fromConversationActions.UnFlagConversation(2);
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState.entities[2].isFlagged).toEqual(false);
        });

        it('should return state', () => {
            const action = new fromConversationActions.UnFlagConversation(3);
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState).toEqual(state);
        });
    });

    describe('STAR|UNSTAR', () => {

        it('should star', () => {
            let conv = conversations[0];
            expect(conv.isStarred).toEqual(false);
            const action = new fromConversationActions.StarConversation({ conversationId: 1, isStarred: true });
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState.entities[1].isStarred).toEqual(true);
        });

        it('should unstar', () => {
            let conv = conversations[1];
            expect(conv.isStarred).toEqual(true);
            const action = new fromConversationActions.UnStarConversation({ conversationId: 2 });
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState.entities[2].isStarred).toEqual(false);
        });

        it('should return state', () => {
            const action = new fromConversationActions.StarConversation({ conversationId: 3, isStarred: true });
            const newState = fromConversationList.ConversationsListReducer(state, action);
            expect(newState).toEqual(state);
        });
    });

    describe('UPDATE_CONTACT', () => {

        it('should update existing conversation', () => {

            const contact = <ContactListDto>{
                firstName: 'John', lastName: 'Doe'
            };

            const action = new fromConversationActions.UpdateConversationContact({ conversation: { id: 1 }, contact: contact });
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual).toEqual({
                ...state,
                entities: {
                    1: { ...conversations[0], contact: contact },
                    2: conversations[1],
                }
            });
        });

        it('should return current state', () => {

            const contact = <ContactListDto>{
                firstName: 'John', lastName: 'Doe'
            };

            const action = new fromConversationActions.UpdateConversationContact({ conversation: { id: 3 }, contact: contact });
            const actual = fromConversationList.ConversationsListReducer(state, action);
            expect(actual).toEqual(state);
        });

    });
});

