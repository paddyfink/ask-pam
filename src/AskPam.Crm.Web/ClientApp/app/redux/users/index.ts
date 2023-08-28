import * as fromUsers from './users.reducer';
import { AppState } from '..';
import { createFeatureSelector, createSelector } from '@ngrx/store';


export {State as UsersState} from './users.reducer';
export {reducer as reducers} from './users.reducer';



export const getUsersState = createFeatureSelector<fromUsers.State>('users');

 export const {
  selectIds: getUserIds,
  selectEntities: getUserEntities,
  selectAll: getAllUsers,
  selectTotal: getUsersTotal,
} = fromUsers.adapter.getSelectors(getUsersState);

