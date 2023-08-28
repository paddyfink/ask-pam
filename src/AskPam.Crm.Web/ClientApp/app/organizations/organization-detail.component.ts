import { Component, OnInit } from '@angular/core';
import { OrganizationService, OrganizationDto, UserDto, IntegrationDto } from '../services/crm.services';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { IntegrationDialogComponent } from './integration.dialog.component';
import { MatDialog } from '@angular/material';
import { CreateUserDialogComponent } from './create-user-dialog.component';

@Component({
    selector: 'app-organization-detail',
    templateUrl: './organization-detail.component.html'
})
export class OrganizationDetailComponent implements OnInit {

    organization: OrganizationDto;
    users: UserDto[];
    integrations: IntegrationDto[];

    constructor(private route: ActivatedRoute,
        public dialog: MatDialog,
        private organizationService: OrganizationService) { }

    openIntegration(): void {
        let dialogRef = this.dialog.open(IntegrationDialogComponent, {
            data: {
                organizationId: this.organization.id
            },
        });
        dialogRef.afterClosed().subscribe((integration) => {
            if (integration) {
                this.integrations.push(integration);
            }
        })
    }

    openUser(): void {
        let dialogRef = this.dialog.open(CreateUserDialogComponent, {
            data: {
                organizationId: this.organization.id
            },
        });
        dialogRef.afterClosed().subscribe((user) => {
            if (user) {
                this.users.push(user);
            }
        })
    }

    ngOnInit() {
        this.route.params.subscribe((params: { id }) => {
            if (params.id) {
                this.organizationService.getOrganizationDetail(params.id)
                    .subscribe(result => {
                        this.organization = result;
                    });

                this.organizationService.getUsers(params.id)
                    .subscribe(
                    result => this.users = result
                    );

                // this.organizationService.getIntegrations(params.id)
                //     .subscribe(result => this.integrations = result
                //     );
            }
        });
    }
}
