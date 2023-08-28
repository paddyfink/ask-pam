import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ContactsPageComponent } from './components/contacts-page/contacts-page.component';
import { ContactDetailPageComponent } from './components/contact-detail-page/contact-detail-page.component';
import { ContactEditPageComponent } from './components/contact-edit-page/contact-edit-page.component';

const routes: Routes = [
	{ path: '', component: ContactsPageComponent, data: { title: 'Contacts' } },
	{ path: 'add', component: ContactEditPageComponent, data: { title: 'New Contact' } },
	{ path: 'edit/:id', component: ContactEditPageComponent, data: { title: 'Edit Contact' } },
	{ path: 'view/:id', component: ContactDetailPageComponent, data: { title: 'Contact' } }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
