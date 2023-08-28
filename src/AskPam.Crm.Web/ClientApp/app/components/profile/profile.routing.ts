import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProfileComponent } from './profile.component';
import { ProfileGeneralComponent } from './general/profile-general.component';
import { ProfileAvatarComponent } from './avatar/profile-avatar.component';
import { ProfilePasswordComponent } from './password/profile-password.component';
import { ProfileEmailComponent } from './email/profile-email.component';
import { NotificationsSettingsComponent } from './notifications/notifications-settings.component';

const routes: Routes = [
	{
		path: '', component: ProfileComponent, data: { title: 'My Profile' }, children: [

			{ path: '', component: ProfileGeneralComponent },
			{ path: 'avatar', component: ProfileAvatarComponent, data: { title: 'Avatar settings' } },
			{ path: 'general', component: ProfileGeneralComponent, data: { title: 'General settings' } },
			{ path: 'email', component: ProfileEmailComponent, data: { title: 'Email settings' } },
			{ path: 'password', component: ProfilePasswordComponent, data: { title: 'Password settings' } },
			{ path: 'notifications', component: NotificationsSettingsComponent, data: { title: 'Notifications' } },
		]
	},
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);