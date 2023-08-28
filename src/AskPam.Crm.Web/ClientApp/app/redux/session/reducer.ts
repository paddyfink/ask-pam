import { Const } from '../../app.consts';
import { Action, createSelector } from '@ngrx/store';
import * as session from './actions';
import { OrganizationDto, ProfileDto } from '../../services/crm.services';
import { merge } from 'lodash';

export interface State {
    profile: ProfileDto;
    roles: string[];
    organizations: OrganizationDto[];
}

const initialState: State = {
    profile: {},
    roles: [],
    organizations: []
};

export function reducer(state = initialState, action: session.Actions): State {
    switch (action.type) {
        case session.GET_SESSION_INFO_SUCCESS:
            return merge({}, state, action.payload);
        // return Object.assign({}, state, { accountIfno: action.payload });

        default:
            return state;
    }
}


// *************************** Selector ****************************
export const getProfile = (state: State) => state.profile;
export const getRoles = (state: State) => state.roles;
export const getOrganizations = (state: State) => state.organizations;

export const getIsAdmin = (state: State) => {
    return state.roles.map(roles => {
        return roles.indexOf(Const.userRoles.admin) > -1;
    });

};

// *************************** PUBLIC API's ****************************
