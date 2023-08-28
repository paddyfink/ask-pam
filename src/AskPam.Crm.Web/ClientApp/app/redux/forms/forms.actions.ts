import { Action } from '@ngrx/store';
import { type } from '../../shared/util'



export const UPDATE = type('[Form] Update');
export const INIT = type('[Form] Init');
export const FORM_SUBMIT_SUCCESS = type('[Form] Submit Success');
export const FORM_SUBMIT_ERROR = type('[Form] Submit error');


export class UpdateForm implements Action {
    type = UPDATE;
    constructor(public payload: {
        entity: string,
        entityId: any,
        form: any
    }) { }
}


export class SubmitFormSucess implements Action {
    type = FORM_SUBMIT_SUCCESS;
    constructor(public payload: {
        entity: string,
        entityId: any,
        result?: any
    }) { }
}

export class SubmitFormError implements Action {
    type = FORM_SUBMIT_ERROR;
    constructor(public payload) { }
}


export type Actions
    = UpdateForm | SubmitFormError | SubmitFormSucess;
