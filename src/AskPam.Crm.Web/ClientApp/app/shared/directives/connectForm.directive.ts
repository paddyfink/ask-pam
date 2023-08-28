import { Input, Output, Directive, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { FormGroupDirective } from '@angular/forms';
import { AppState } from '../../redux';
import { Store } from '@ngrx/store';
import { Actions } from '@ngrx/effects';
import * as FormsAction from '../../redux/forms/forms.actions';
import { isEqual } from 'lodash';
import 'rxjs/add/operator/take';

@Directive({
	selector: '[appConnectForm]',
})
export class ConnectFormDirective implements OnInit, OnDestroy {
	@Input() entity: string;
	@Input() entityId: any;
	@Input() debounce = 300;
	@Output() error = new EventEmitter();
	@Output() success = new EventEmitter();
	formChange: Subscription;
	formSuccess: Subscription;
	formError: Subscription;

	constructor(private formGroupDirective: FormGroupDirective,
		private actions$: Actions,
		private store: Store<AppState>) {
	}

	ngOnInit() {

		this.store.select(state => state.forms[this.entity]).map(m => m[this.entityId])
			.take(1)
			.subscribe(formValue => {
				if (formValue) {
					if (!isEqual(this.formGroupDirective.form.value, formValue)) {
						this.formGroupDirective.form.patchValue(formValue);
					}
				}
			});

		this.formChange = this.formGroupDirective.form.valueChanges
			.debounceTime(this.debounce).subscribe(value => {
				this.store.dispatch(new FormsAction.UpdateForm({
					entity: this.entity,
					entityId: this.entityId,
					form: value
				}));
			});

		this.formSuccess = this.actions$
			.ofType(FormsAction.FORM_SUBMIT_SUCCESS)
			.filter((action: FormsAction.Actions) => {
				return action.payload.entity === this.entity && action.payload.entityId === this.entityId;
			})
			.subscribe((action: FormsAction.Actions) => {
				this.success.emit(action.payload.result);
			});

		this.formError = this.actions$
			.ofType(FormsAction.FORM_SUBMIT_ERROR)
			.filter((action: FormsAction.Actions) => action.payload.entity === this.entity && action.payload.entityId === this.entityId)
			.subscribe((action: FormsAction.Actions) => this.error.emit(action.payload.error));
	}

	ngOnDestroy() {
		this.formChange.unsubscribe();
		this.formError.unsubscribe();
		this.formSuccess.unsubscribe();
	}
}
