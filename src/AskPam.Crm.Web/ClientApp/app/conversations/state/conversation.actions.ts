import { Action } from "@ngrx/store";
import {
  ChannelDto,
  ConversationDto,
  ConversationListDto,
  ConversationListRequestDto,
  EnumValueDto,
  MessageDto,
  PagedResultDtoOfConversationListDto,
  SendMessageDto,
  ContactListDto,
  ContactDto,
  UserDto
} from "../../services/crm.services";
import { type } from "../../shared/util";

export const ConversationActionTypes = {
  LOAD: type("[Conversation] Load"),
  LOADED: type("[Conversation] Loaded"),
  RESET: type("[Conversation] Reset"),
  ADD_MESSAGE: type("[Conversation] Add Message"),
  MARK_AS_READ: type("[Conversation] Mask  as read"),
  MARK_AS_UNREAD: type("[Conversation] Mask  as unread"),
  SEND_MESSAGE: type("[Conversation] Send Message"),
  SEND_MESSAGE_SUCCESS: type("[Conversation] Send Message Success"),
  FLAG: type("[Conversation] Flag"),
  FLAG_SUCCESS: type("[Conversation] Flag Success"),
  FLAG_ERROR: type("[Conversation] Flag Error"),
  STAR: type("[Conversation] Star"),
  UNSTAR: type("[Conversation] Unstar"),
  ARCHIVE: type("[Conversation] Archive"),
  ARCHIVE_SUCCESS: type("[Conversation] Archive Sucess"),
  ARCHIVE_ERROR: type("[Conversation] Archive Error"),
  ASSIGN: type("[Conversation] Assign"),
  UPDATE_CONTACT: type("[Conversation] Update contact"),
  FLAG_CONFIRM_DIALOG_OPEN: "[Converation] flag confirm dialog open",
  UNLINK_CHANNEL_CONTACT: "[Conversation] Unlink channel",
  UPDATE_CONTACT_SUCCESS: "[Conversation] Unlink Contact",
  TOGGLE_BOT_ACTIVATION: "[Conversation] Toggle bot activation"
};

export class LoadConversation implements Action {
  type = ConversationActionTypes.LOAD;
  constructor(public payload: number) {}
}

export class ConverationLoaded implements Action {
  type = ConversationActionTypes.LOADED;
  constructor(public payload: ConversationDto) {}
}

export class ResetConversation implements Action {
  type = ConversationActionTypes.RESET;
}

export class AddMessage implements Action {
  type = ConversationActionTypes.ADD_MESSAGE;
  constructor(public payload: ConversationListDto) {}
}

export class MarkConversationAsRead implements Action {
  type = ConversationActionTypes.MARK_AS_READ;
  constructor(public payload: number) {}
}

export class MarkConversationAsUnread implements Action {
  type = ConversationActionTypes.MARK_AS_UNREAD;
  constructor(public payload: number) {}
}

export class SendMessage implements Action {
  type = ConversationActionTypes.SEND_MESSAGE;
  constructor(public payload: SendMessageDto) {}
}

export class SendMessageSuccess implements Action {
  type = ConversationActionTypes.SEND_MESSAGE_SUCCESS;
  constructor(public payload: MessageDto) {}
}

export class FlagConversation implements Action {
  type = ConversationActionTypes.FLAG;
  constructor(public payload: number) {}
}

export class FlagConversationSuccess implements Action {
  type = ConversationActionTypes.FLAG_SUCCESS;
  constructor(public payload: number) {}
}

export class FlagConversationError implements Action {
  type = ConversationActionTypes.FLAG_ERROR;
  constructor(public payload: number) {}
}

export class StarConversation implements Action {
  type = ConversationActionTypes.STAR;
  constructor(
    public payload: {
      conversationId: number;
      isStarred: boolean;
    }
  ) {}
}

export class UnStarConversation implements Action {
  type = ConversationActionTypes.UNSTAR;
  constructor(
    public payload: {
      conversationId: number;
    }
  ) {}
}

export class ArchiveConversation implements Action {
  type = ConversationActionTypes.ARCHIVE;
  constructor(public payload: number) {}
}

export class ArchiveSuccess implements Action {
  type = ConversationActionTypes.ARCHIVE_SUCCESS;
  constructor(public payload: number) {}
}

export class ArchiveError implements Action {
  type = ConversationActionTypes.ARCHIVE_ERROR;
  constructor(public payload: number) {}
}

export class AssignConversation implements Action {
  type = ConversationActionTypes.ASSIGN;
  constructor(
    public payload: {
      conversationId: number;
      user: UserDto;
    }
  ) {}
}

export class UpdateConversationContact implements Action {
  type = ConversationActionTypes.UPDATE_CONTACT;
  constructor(
    public payload: {
      contact: ContactListDto | ContactDto;
      conversation: ConversationDto;
    }
  ) {}
}

export class UpdateConversationContactSuccess implements Action {
  type = ConversationActionTypes.UPDATE_CONTACT_SUCCESS;
  constructor(
    public payload: { conversationId: number; contact: ContactDto }
  ) {}
}

export class UnlinkConversationContact implements Action {
  type = ConversationActionTypes.UNLINK_CHANNEL_CONTACT;
  constructor(public payload: { conversation: ConversationDto }) {}
}

export class FlagConversationConfirmDialogOpen implements Action {
  readonly type = ConversationActionTypes.FLAG_CONFIRM_DIALOG_OPEN;
  constructor(
    public payload: {
      cancel?: Action;
      action: Action;
      text?: string;
      title?: string;
      isFlagged?: boolean;
    }
  ) {}
}

export class ToggleBotActivation implements Action {
  type = ConversationActionTypes.TOGGLE_BOT_ACTIVATION;
  constructor(public payload: number) {}
}

export type ConversationActions =
  | LoadConversation
  | ConverationLoaded
  | ResetConversation
  | AddMessage
  | MarkConversationAsRead
  | MarkConversationAsUnread
  | SendMessage
  | SendMessageSuccess
  | FlagConversation
  | FlagConversationSuccess
  | FlagConversationError
  | StarConversation
  | ArchiveConversation
  | ArchiveSuccess
  | ArchiveError
  | AssignConversation
  | UpdateConversationContact
  | FlagConversationConfirmDialogOpen
  | UnlinkConversationContact
  | UpdateConversationContactSuccess
  | ToggleBotActivation;
