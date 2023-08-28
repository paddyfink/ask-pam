import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

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

const routes: Routes = [
    {
        path: '', component: SettingsComponent, data: { title: 'Settings' }, children: [
            { path: '', component: SettingsOverviewComponent, data: { title: 'General' } },
            { path: 'email', component: EmailSettingsComponent, data: { title: 'Email' } },
            { path: 'facebook', component: FacebookSettingsComponent, data: { title: 'Facebook' } },
            { path: 'widget', component: WidgetSettingsComponent, data: { title: 'Widget' } },
            { path: 'general', component: SettingsGeneralComponent, data: { title: 'General' } },
            { path: 'users', component: SettingsUsersComponent, data: { title: 'Users' } },
            { path: 'users/invite', component: SettingsInviteUserComponent, data: { title: 'Invite a user' } },
            { path: 'tags', component: SettingsTagsComponent, data: { title: 'Tags' } },
            { path: 'qna-bot', component: QnaBotContainerComponent, data: { title: 'QnA Bot' } },

        ]
    }
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);