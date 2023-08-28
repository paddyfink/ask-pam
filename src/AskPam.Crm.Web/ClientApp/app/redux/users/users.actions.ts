import { Action } from '@ngrx/store';
import { type } from '../../shared/util';
import { UserDto, PagedResultDtoOfUserDto } from '../../services';


export const UsersActionTypes = {
	LOAD: type('[Users] Load'),
	ADD: type('[Users] Add'),
	UPDATE: type('[Users] Update'),
	LOADED: type('[Users] Loaded')
};

export class LoadUsers implements Action {
	type = UsersActionTypes.LOAD;
}

export class AddUser implements Action {
	type = UsersActionTypes.ADD;
	constructor(public payload: UserDto) { }
}
export class UpdateUser implements Action {
	type = UsersActionTypes.UPDATE;
	constructor(public payload: UserDto) { }
}

export class UsersLoaded implements Action {
	type = UsersActionTypes.LOADED;
	constructor(public payload: PagedResultDtoOfUserDto) { }
}

export type Actions = LoadUsers
	| UsersLoaded;
