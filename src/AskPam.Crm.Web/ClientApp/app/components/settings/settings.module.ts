import { NgModule } from '@angular/core';

import { SettingsComponent } from './settings.component';
import { SettingsOverviewComponent } from './settings-overview.component';
import { EmailSettingsComponent } from './channels/email-settings.component';
import { EmailSettingsDialogComponent } from './channels/email-settings-dialog.component';
import { WidgetSettingsComponent } from './channels/widget-settings.component';
import { SettingsGeneralComponent } from './general/settings-general.component';
import { SettingsUsersComponent } from './users/settings-users.component';
import { SettingsInviteUserComponent } from './users/settings-invite-user.component';
import { SettingsTagsComponent } from './tags/settings-tags.component';
import { SettingsTagDialogComponent } from './tags/settings-tags.dialog.component';
import { QnaBotContainerComponent } from './qna-bot/qna-bot-container.component';
import { QnaBotSettingsComponent } from './qna-bot/qna-bot-settings.component';
import { QnaBotPairComponent } from './qna-bot/qna-bot-pair.component';
import { QnaBotTestComponent } from './qna-bot/qna-bot-test.component';
import { FacebookSettingsComponent } from './channels/facebook-settings.component';
import { routing } from './settings.routing';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
	declarations: [
		SettingsComponent,
		SettingsOverviewComponent,
		EmailSettingsComponent,
		EmailSettingsDialogComponent,
		WidgetSettingsComponent,
		SettingsGeneralComponent,
		SettingsUsersComponent,
		SettingsInviteUserComponent,
		SettingsTagsComponent,
		SettingsTagDialogComponent,
		QnaBotContainerComponent,
		QnaBotSettingsComponent,
		QnaBotPairComponent,
		QnaBotTestComponent,
		FacebookSettingsComponent,
	],
	entryComponents: [
		SettingsTagDialogComponent,
		EmailSettingsDialogComponent,
	],
	imports: [
		SharedModule,
		routing
	],
	exports: [],
	providers: [],
})
export class SettingsModule { }