import { SharedModule } from '../../shared/shared.module';
import { NgModule } from '@angular/core';

import { CKEditorModule } from 'ng2-ckeditor';

import { ProfileComponent } from './profile.component';
import { ProfileGeneralComponent } from './general/profile-general.component';
import { ProfileAvatarComponent } from './avatar/profile-avatar.component';
import { ProfilePasswordComponent } from './password/profile-password.component';
import { ProfileEmailComponent } from './email/profile-email.component';
import { NotificationsSettingsComponent } from './notifications/notifications-settings.component';

import { routing } from './profile.routing'
//import { TagsService } from '../../services/crm.services';

@NgModule({
	imports: [
		SharedModule,
		routing,
		CKEditorModule
	],
	declarations: [
		ProfileComponent,
		ProfileGeneralComponent,
		ProfileAvatarComponent,
		ProfilePasswordComponent,
		ProfileEmailComponent,
		NotificationsSettingsComponent
	],
	providers: [],
	exports: []
})
export class ProfileModule { }