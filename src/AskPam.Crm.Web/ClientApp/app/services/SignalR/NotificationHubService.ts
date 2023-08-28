import { Injectable, EventEmitter, NgZone } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';
import { NotificationDto, ProfileDto } from '../crm.services';
import { Auth } from '../auth.service';
import { Store } from '@ngrx/store';
import { AppState } from '../../redux/index';
import { AddNotification } from '../../redux/notifications/notifications.actions';
import { InAppNotificationService } from '../../shared/inAppNotification.service';


@Injectable()
export class NotificationHubService {


	public connectionEstablished: EventEmitter<Boolean> = new EventEmitter();

	public currentUser: ProfileDto;

	private connection: HubConnection;

	constructor(private store: Store<AppState>,
		private authService: Auth,
		private zone: NgZone,
		private inAppNotificationService: InAppNotificationService) {
		this.store.select(state => state.session.profile).subscribe(profile => {
			this.currentUser = profile;
		});
	}

	init() {
		this.connection = new HubConnection('/notification');

		this.registerOnServerEvents();

		this.startConnection();
	}

	private startConnection(): void {
		let organizationId = this.authService.getUserOrganization();

		this.connection.start()
			.then(() => {
				this.connection.send('JoinGroup', organizationId);
				this.connection.send('JoinGroup', this.currentUser.id);
			})
			.catch(err => {
				console.log('Error while establishing connection');
			});
	}

	private registerOnServerEvents(): void {

		this.connection.on('newNotification', (notification: NotificationDto) => {
			console.log('New notification');
			this.store.dispatch(new AddNotification(notification));
			this.inAppNotificationService.info(notification.description);
		});
	}


}

