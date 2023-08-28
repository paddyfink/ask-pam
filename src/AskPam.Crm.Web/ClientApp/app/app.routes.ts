import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';

import { MainComponent } from './main/main.component';
import { AdminGuard, AuthGuard, HostGuard } from './core/guards';

// Dashboard


const appRoutes: Routes = [
    { path: 'auth', loadChildren: './auth/auth.module#AuthModule' },
    {
        path: '', component: MainComponent, canActivate: [AuthGuard], canActivateChild: [AuthGuard], children: [
            { path: '', component: DashboardComponent, data: { title: 'Dashboard' } },
            { path: 'home', component: DashboardComponent, data: { title: 'Dashboard' } },
            { path: 'contacts', loadChildren: './contacts/contacts.module#ContactsModule', },
            { path: 'library', loadChildren: './library/library.module#LibraryModule', },
            { path: 'conversations', loadChildren: './conversations/conversations.module#ConversationsModule', },

            { path: 'profile', loadChildren: './components/profile/profile.module#ProfileModule' },
            { path: 'settings', loadChildren: './components/settings/settings.module#SettingsModule' },
            {
                path: 'admin', canActivate: [HostGuard], canActivateChild: [HostGuard], children: [
                    { path: 'organizations', loadChildren: './organizations/organizations.module#OrganizationsModule' },
                    { path: 'configurations', loadChildren: './configurations/configurations.module#ConfigurationsModule' },
                ],
            }
        ]
    }
    // { path: '**', redirectTo: 'home' },
];

export const appRoutingProviders: any[] = [
    AuthGuard, AdminGuard, HostGuard
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
