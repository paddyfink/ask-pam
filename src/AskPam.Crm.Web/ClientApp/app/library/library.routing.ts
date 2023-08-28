import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LibraryPageComponent } from './library-page/library-page.component';
import { LibraryDetailPageComponent } from './library-detail-page/library-detail-page.component';
import { LibraryEditPageComponent } from './library-edit-page/library-edit-page.component';

const routes: Routes = [
	{ path: '', component: LibraryPageComponent, data: { title: 'Library' } },
	{ path: 'add', component: LibraryEditPageComponent, data: { title: 'New Item' } },
	{ path: 'edit/:id', component: LibraryEditPageComponent, data: { title: 'Edit Item' } },
	{ path: 'view/:id', component: LibraryDetailPageComponent, data: { title: 'Library Item' } }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);