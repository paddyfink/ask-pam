import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent, ForgetPasswordComponent } from './index';


const routes: Routes = [
	{ path: 'login', component: LoginComponent, data: { title: 'Login' } },
    { path: 'forgot-your-password', component: ForgetPasswordComponent, data: { title: 'Forgot your password' } },
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);