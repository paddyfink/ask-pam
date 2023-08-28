import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import {OrganizationAddComponent} from './organization-add.component';
import {OrganizationDetailComponent} from './organization-detail.component';
import {OrganizationListComponent} from './organization-list.component';

const routes: Routes = [
        { path: '', component: OrganizationListComponent, data: { title: 'Organizations' } },
        { path: 'add', component: OrganizationAddComponent, data: { title: 'Organizations' }},
        { path: 'view/:id', component: OrganizationDetailComponent, data: { title: 'Organizations' } },
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);