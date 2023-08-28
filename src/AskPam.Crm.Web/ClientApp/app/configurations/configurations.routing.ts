import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ConfigurationsComponent } from './configurations.component';

const routes: Routes = [
	{ path: '', component: ConfigurationsComponent, data: { title: 'Configurations' } },
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);