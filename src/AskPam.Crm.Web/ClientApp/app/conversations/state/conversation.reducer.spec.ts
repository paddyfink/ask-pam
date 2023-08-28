import { ChannelDto } from '../../services/crm.services';
import * as fromConversationActions from './conversation.actions';
import * as fromConversation from './conversation.reducer';

let state: fromConversation.State;
const conversation = {
    id: 1,
    messages: [],
    contact: null
};

describe('ConversationReducer', () => {

    beforeEach(() => {
        state = {
            loaded: true,
            loading: false,
            item: conversation,
            error: {}
        };
    });

    describe('LOAD', () => {

        it('should set loading to true', () => {
            const { initialState } = fromConversation;
            const action = new fromConversationActions.LoadConversation(1);
            const actual = fromConversation.reducer(initialState, action);
            expect({ ...actual }).toEqual({ ...initialState, loading: true });
        });
    });

    describe('LOADED', () => {

        it('should populate entity', () => {
            const { initialState } = fromConversation;
            const action = new fromConversationActions.ConverationLoaded(conversation);
            const actual = fromConversation.reducer(initialState, action);
            expect({ ...actual }).toEqual({
                ...initialState,
                loaded: true,
                loading: false,
                item: conversation,
                error: {}
            });
        });
    });

    describe('RESET', () => {
        it('should restore initial sate', () => {
            const { initialState } = fromConversation;
            const action = new fromConversationActions.ResetConversation();
            const actual = fromConversation.reducer(state, action);
            expect(actual).toEqual(initialState);
        });

    });

    describe('Archive', () => {
        it('should restore initial sate', () => {
            const { initialState } = fromConversation;
            const action = new fromConversationActions.ArchiveConversation(1);
            const actual = fromConversation.reducer(state, action);
            expect(actual).toEqual(initialState);
        });

        it('should restore current sate', () => {
            const action = new fromConversationActions.ArchiveConversation(2);
            const actual = fromConversation.reducer(state, action);
            expect(actual).toEqual(state);
        });

    });

    describe('ADD_MESSAGE', () => {

        const conv = {
            id: 1,
            lastMessage: {
                id: 1, conversationId: 1, seen: false, attachmentsCount: 0, text: 'new', date: new Date(), author: 'John Doe'
            },
            contact: null,
            seen: true,
            organizationId: 'id',
            isFlagged: true,
            isStarred: true,
            date: new Date()
        };
        it('should add message', () => {
            const action = new fromConversationActions.AddMessage(conv);
            const actual = fromConversation.reducer(state, action);
            expect(actual.item.messages).toEqual([conv.lastMessage]);
        });

        it('when conversation different should restore current sate', () => {
            const action = new fromConversationActions.AddMessage({ ...conv, id: 2 });
            const actual = fromConversation.reducer(state, action);
            expect(actual).toEqual(state);
        });

        it('when message already present should restore current sate', () => {
            state = {
                ...state,
                item: { ...state.item, messages: [conv.lastMessage] }
            };
            const action = new fromConversationActions.AddMessage(conv);
            const actual = fromConversation.reducer(state, action);
            expect(actual).toEqual(state);
        });
    });

    describe('UPDATE_CHANNEL_CONTACT_SUCCESS', () => {

        it('should update contact', () => {
            const contact = { id: 1, firstName: 'John', lastName: 'Doe', isNew: false, isAlreadyContacted: true };

            const action = new fromConversationActions.UpdateConversationContactSuccess({ contact: contact, conversationId: 1 });
            const newState = fromConversation.reducer(state, action);

            expect(newState.item.contact).toEqual(contact);
        });

        it('should return state', () => {
            const contact = { id: 1, firstName: 'John', lastName: 'Doe', isNew: false, isAlreadyContacted: true };

            const action = new fromConversationActions.UpdateConversationContactSuccess({ contact: contact, conversationId: 3 });
            const newState = fromConversation.reducer(state, action);

            expect(newState).toEqual(state);
        });
    });

    describe('FLAG|UNFLAG', () => {

        it('should flag', () => {
            state = {
                ...state,
                item: {
                    ...state.item, isFlagged: false
                }
            };
            expect(state.item.isFlagged).toEqual(false);
            const action = new fromConversationActions.FlagConversation(1);
            const newState = fromConversation.reducer(state, action);
            expect(newState.item.isFlagged).toEqual(true);
        });

        it('should unflag', () => {
            state = {
                ...state,
                item: {
                    ...state.item, isFlagged: true
                }
            };
            expect(state.item.isFlagged).toEqual(true);
            const action = new fromConversationActions.UnFlagConversation(1);
            const newState = fromConversation.reducer(state, action);
            expect(newState.item.isFlagged).toEqual(false);
        });

        it('should return state', () => {
            const action = new fromConversationActions.UnFlagConversation(2);
            const newState = fromConversation.reducer(state, action);
            expect(newState).toEqual(state);
        });
    });

    describe('STAR|UNSTAR', () => {

        it('should star', () => {
            state = {
                ...state,
                item: {
                    ...state.item, isStarred: false
                }
            };
            expect(state.item.isStarred).toEqual(false);
            const action = new fromConversationActions.StarConversation({ conversationId: 1, isStarred: true });
            const newState = fromConversation.reducer(state, action);
            expect(newState.item.isStarred).toEqual(true);
        });

        it('should unflag', () => {
            state = {
                ...state,
                item: {
                    ...state.item, isStarred: true
                }
            };
            expect(state.item.isStarred).toEqual(true);
            const action = new fromConversationActions.UnStarConversation({ conversationId: 1 });
            const newState = fromConversation.reducer(state, action);
            expect(newState.item.isStarred).toEqual(false);
        });

        it('should return state', () => {
            const action = new fromConversationActions.StarConversation({ conversationId: 2, isStarred: true });
            const newState = fromConversation.reducer(state, action);
            expect(newState).toEqual(state);
        });
    });

    describe('TOGGLE_BOT_ACTIVATION', () => {

        it('should activate', () => {
            state = {
                ...state,
                item: {
                    ...state.item, botDisabled: false
                }
            };
            expect(state.item.botDisabled).toEqual(false);
            const action = new fromConversationActions.ToggleBotActivation(1);
            const newState = fromConversation.reducer(state, action);
            expect(newState.item.botDisabled).toEqual(true);
        });

        it('should unactivate', () => {
            state = {
                ...state,
                item: {
                    ...state.item, botDisabled: true
                }
            };
            expect(state.item.botDisabled).toEqual(true);
            const action = new fromConversationActions.ToggleBotActivation(1);
            const newState = fromConversation.reducer(state, action);
            expect(newState.item.botDisabled).toEqual(false);
        });

        it('should return state', () => {
            const action = new fromConversationActions.StarConversation({ conversationId: 2, isStarred: true} );
            const newState = fromConversation.reducer(state, action);
            expect(newState).toEqual(state);
        });
    });
});
