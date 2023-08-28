import { Component, Input, SimpleChanges, OnChanges } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { Store } from '@ngrx/store';
import { AppState } from '../redux';
import { Helper } from '../shared/helper';

import { FollowersService, UserDto } from '../services/crm.services';
import { getAllUsers } from '../redux/users';



@Component({
	selector: 'app-followers',
	styleUrls: ['./followers.component.scss'],
	templateUrl: './followers.component.html'
})
export class FollowersComponent implements OnChanges {

	users: UserDto[];
	filteredUsers: UserDto[];
	followers: UserDto[];
	@Input() conversationId: number;

	constructor(
		private store: Store<AppState>,
		private followersService: FollowersService,
		private helper: Helper,
	) {
		this.store.select(getAllUsers).subscribe(users => {
			this.users = users;
		});
	}

	filterUsers(value: string): void {
		let f = this.users.filter(user => {
			return !this.followers.find(follower => {
				return user.id === follower.id;
			});
		});

		this.filteredUsers = f.filter((item: any) => {

			if (value) {
				return item.fullName.toLowerCase().indexOf(value.toLowerCase()) > -1;
			} else {
				return false;
			}
		});
	}

	ngOnChanges(changes: SimpleChanges) {
		let conversationId = changes['conversationId'].currentValue;
		if (conversationId) {
			this.loadFollowers(conversationId);
		}
	}

	onFollowerAdded(user: IFollower): void {
		this.followersService.follow({
			userId: user.id,
			conversationId: this.conversationId
		})
			.subscribe(result => {
			}, error => {
				this.helper.displayError(error.message);
			});
	}
	onFollowerRemoved(user: IFollower): void {
		this.followersService.unfollow({
			userId: user.id,
			conversationId: this.conversationId
		})
			.subscribe(result => {
			}, error => {
				this.helper.displayError(error.message);
			});
	}

	loadFollowers(conversationId: number): void {
		this.followersService.getFollowers(conversationId)
			.subscribe(result => {
				this.followers = result;
			}, error => {
				this.helper.displayError(error.message);
			});
	}
}

export interface IFollower {
	id: string;
	fullName: string;
	picture: string;
}
