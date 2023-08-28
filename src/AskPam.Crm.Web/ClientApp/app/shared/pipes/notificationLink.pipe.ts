import { Pipe, PipeTransform } from '@angular/core';
import { NotificationDto } from '../../services/crm.services';

@Pipe({
	name: 'notificationLink'
})
export class NotificationLinkPipe implements PipeTransform {

    transform(notification: NotificationDto) {
        switch (notification.notificationType) {
            case "NewMessage":
                return "/conversations/" + notification.entityId;
            case "ConversationAssigned":
                return "/conversations/" + notification.entityId;
            case "FollowConversation":
                return "/conversations/" + notification.entityId;
            case "LibraryCreated":
                return "/library/" + notification.entityId;
        }
	}

}