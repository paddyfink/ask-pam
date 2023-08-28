import { Component, OnInit } from '@angular/core';

import { OrganizationService, CreateOrganizationDto } from '../services/crm.services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';

@Component({
	selector: 'organization-add',
	templateUrl: './organization-add.component.html',
	//styleUrls: ['./conversation.component.scss'],
	providers: [OrganizationService]
})
export class OrganizationAddComponent implements OnInit {

	formGroup: FormGroup;

	constructor(
		private organizationService: OrganizationService,
		private fb: FormBuilder, 
		private dialogRef: MatDialogRef<OrganizationAddComponent>) {
	}

	add(): void {
		this.organizationService.createOrganization(this.formGroup.value).subscribe((result) => {
			this.dialogRef.close(result);
		});
	}

	ngOnInit(): void {
		this.dialogRef.updateSize('40%');

		this.formGroup = this.fb.group({
			'name': ['', Validators.required],
		});
	}

}