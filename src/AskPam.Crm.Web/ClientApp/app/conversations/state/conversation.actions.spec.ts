import * as actions from './conversation.actions';

const conversation = {
    id: 1,
    messages: [],
    contact: null
};

describe('Conversation Actions', () => {
    describe('LoadConversation', () => {
        it('should create an action', () => {
            const action = new actions.LoadConversation(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.LOAD,
                payload: 1
            });
        });
    });

    describe('ConverationLoaded', () => {
        it('should create an action', () => {
            const action = new actions.ConverationLoaded(conversation);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.LOADED,
                payload: conversation
            });
        });
    });

    describe('ResetConversation', () => {

        it('should create an action', () => {
            const action = new actions.ResetConversation();
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.RESET
            });
        });
    });

    describe('AddMessage', () => {
        const conv = {
            id: 1,
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

        it('should create an action', () => {
            const action = new actions.AddMessage(conv);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.ADD_MESSAGE,
                payload: conv
            });
        });
    });

    describe('MarkConversationAsRead', () => {

        it('should create an action', () => {
            const action = new actions.MarkConversationAsRead(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.MARK_AS_READ,
                payload: 1
            });
        });
    });

    describe('MarkConversationAsUnread', () => {

        it('should create an action', () => {
            const action = new actions.MarkConversationAsUnread(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.MARK_AS_UNREAD,
                payload: 1
            });
        });
    });

    describe('SendMessage', () => {
        const message = {
            id: 1, conversationId: 3, seen: false, attachmentsCount: 0, text: 'new', date: new Date(), author: 'John Doe',
            type: 'text', isBodyHtml: false
        };

        it('should create an action', () => {
            const action = new actions.SendMessage(message);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.SEND_MESSAGE,
                payload: message
            });
        });
    });


    describe('SendMessageSuccess', () => {
        const message = {
            id: 1, conversationId: 3, seen: false, attachmentsCount: 0, text: 'new', date: new Date(), author: 'John Doe',
            type: 'text', isBodyHtml: false
        };
        it('should create an action', () => {
            const action = new actions.SendMessageSuccess(message);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.SEND_MESSAGE_SUCCESS,
                payload: message
            });
        });
    });

    describe('FlagConversation', () => {
        it('should create an action', () => {
            const action = new actions.FlagConversation(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.FLAG,
                payload: 1
            });
        });
    });

    describe('UnFlagConversation', () => {
        it('should create an action', () => {
            const action = new actions.UnFlagConversation(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.UNFLAG,
                payload: 1
            });
        });
    });

    describe('StarConversation', () => {
        it('should create an action', () => {
            const action = new actions.StarConversation({ conversationId: 1, isStarred: true });
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.STAR,
                payload: { conversationId: 1, isStarred: true }
            });
        });
    });

    describe('UnStarConversation', () => {
        it('should create an action', () => {
            const action = new actions.UnStarConversation({ conversationId: 1 });
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.UNSTAR,
                payload: { conversationId: 1 }
            });
        });
    });

    describe('ArchiveConversation', () => {
        it('should create an action', () => {
            const action = new actions.ArchiveConversation(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.ARCHIVE,
                payload: 1
            });
        });
    });

    describe('UnArchiveConversation', () => {
        it('should create an action', () => {
            const action = new actions.UnArchiveConversation(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.UNARCHIVE,
                payload: 1
            });
        });
    });

    describe('AssignConversation', () => {
        it('should create an action', () => {
            const action = new actions.AssignConversation({ conversationId: 1, user: { id: '1' } });
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.ASSIGN,
                payload: { conversationId: 1, user: { id: '1' } }
            });
        });
    });

    describe('UpdateConversationContact', () => {
        const payload = {
            contact: { id: 1, firstName: '', lastName: '', groupId: null, createdAt: null, isNew: true },
            conversation: conversation
        };
        it('should create an action', () => {
            const action = new actions.UpdateConversationContact(payload);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.UPDATE_CONTACT,
                payload: payload
            });
        });
    });

    describe('UpdateConversationContactSuccess', () => {
        const payload = {
            contact: { id: 1, firstName: '', lastName: '', groupId: null, createdAt: null, isNew: true },
            conversationId: 1
        };
        it('should create an action', () => {
            const action = new actions.UpdateConversationContactSuccess(payload);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.UPDATE_CONTACT_SUCCESS,
                payload: payload
            });
        });
    });

    describe('UnlinkConversationContact', () => {

        it('should create an action', () => {
            const action = new actions.UnlinkConversationContact(null);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.UNLINK_CHANNEL_CONTACT,
                payload: null
            });
        });
    });

    describe('ToggleBotActivation', () => {

        it('should create an action', () => {
            const action = new actions.ToggleBotActivation(1);
            expect({ ...action }).toEqual({
                type: actions.ConversationActionTypes.TOGGLE_BOT_ACTIVATION,
                payload: 1
            });
        });
    });

});
