import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { OrganizationAddComponent } from './organization-add.component';
import { OrganizationDetailComponent } from './organization-detail.component';
import { OrganizationListComponent } from './organization-list.component';
import { routing } from './organizations.routing';
import { OrganizationService } from '../services/crm.services';
import { IntegrationDialogComponent } from './integration.dialog.component';
import { CreateUserDialogComponent } from './create-user-dialog.component';

@NgModule({
    entryComponents: [
        IntegrationDialogComponent,
        CreateUserDialogComponent,
        OrganizationAddComponent
    ],
    declarations: [
        IntegrationDialogComponent,
        OrganizationAddComponent,
        OrganizationDetailComponent,
        OrganizationListComponent,
        CreateUserDialogComponent
    ],
    imports: [
        SharedModule,
        routing
    ],
    exports: [],
    providers: [OrganizationService],
})
export class OrganizationsModule { }