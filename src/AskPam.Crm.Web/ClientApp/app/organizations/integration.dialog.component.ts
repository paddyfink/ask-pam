import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { Helper } from '../shared/helper';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TdLoadingService, TdMessageComponent } from '@covalent/core';
import { IntegrationDto, OrganizationService } from '../services/crm.services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';


@Component({
	selector: 'app-integration-dialog',
	templateUrl: './integration.dialog.component.html',
})

export class IntegrationDialogComponent implements OnInit {

	@ViewChild(TdMessageComponent) messageComponent: TdMessageComponent;

	formGroup: FormGroup;
	integration: IntegrationDto;
	error: string;
	channelTypes: { value: number, viewValue: string }[] = [
		{ value: 0, viewValue: 'Email' },
		{ value: 1, viewValue: 'Sms' },
		{ value: 4, viewValue: 'Web Widget' },
	];

	constructor(
		private loadingService: TdLoadingService,
		private fb: FormBuilder,
		private organizationService: OrganizationService,
		private dialogRef: MatDialogRef<IntegrationDialogComponent>,
		@Inject(MAT_DIALOG_DATA) private data: any) {
	}

	submit(): void {
		this.loadingService.register('integration.edit');
		this.organizationService.createIntegration(this.data.organizationId, this.formGroup.value)
			.subscribe(
				result => {
					this.loadingService.resolve('integration.edit');
					this.dialogRef.close(result);
				},
				error => {
					this.error = error;
					// this.messageComponent.open();
					this.loadingService.resolve('integration.edit');
				}
			);
	}

	cancel() {
		this.dialogRef.close();
	}

	ngOnInit(): void {
		this.dialogRef.updateSize('40%');

		this.formGroup = this.fb.group({
			'organizationId': this.data.organizationId,
			'name': ['', Validators.required],
			'channelTypeId': 0,
			'secret': '',
			'username': ['', Validators.required],
			'token': ['']
		});
	}
}
