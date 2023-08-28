import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { TdMessageComponent, TdLoadingService } from '@covalent/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { OrganizationService } from '../services/crm.services';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'app-create-user-dialog',
    templateUrl: './create-user-dialog.component.html'
})
export class CreateUserDialogComponent implements OnInit {

    @ViewChild(TdMessageComponent) messageComponent: TdMessageComponent;
    formGroup: FormGroup;
    error: any;
    roles: { value: string, viewValue: string }[] = [
        { value: 'Admin', viewValue: 'Admin' },
        { value: 'User', viewValue: 'User' },
        { value: 'Reader', viewValue: 'Reader' },
    ];

    constructor(
        private loadingService: TdLoadingService,
        private fb: FormBuilder,
        private organizationService: OrganizationService,
        private dialogRef: MatDialogRef<CreateUserDialogComponent>,
        @Inject(MAT_DIALOG_DATA) private data: any
    ) { }

    submit(): void {
        this.loadingService.register('loading');
        this.organizationService.addUser(this.data.organizationId, this.formGroup.value)
            .subscribe(
            result => {
                this.loadingService.resolve('loading');
                this.dialogRef.close(result);
            },
            error => {
                this.error = error;
                this.messageComponent.open();
                this.loadingService.resolve('loading');
            }
            )
    }

    cancel() {
        this.dialogRef.close();
    }
    ngOnInit() {
        this.dialogRef.updateSize('40%');

        this.formGroup = this.fb.group({
            'organizationId': this.data.organizationId,
            'firstName': '',
            'lastName': '',
            'email': ['', [Validators.required, Validators.email]],
            'roleName': ['', Validators.required],
            'password': ''
        });
    }
}