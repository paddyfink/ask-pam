import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { ConverationsGuard } from './conversationsGuard';
import { MassMessagingComponent } from './components/mass-messaging/mass-messaging.component';
import { EmailDetailsComponent } from './components/email-detail/email-details.component';
import { ConversationsComponent } from './components/conversations/conversations.component';

const routes: Routes = [
	{ path: 'messaging', component: MassMessagingComponent, data: { title: 'Conversations' } },
	{ path: 'email/view/:id', component: EmailDetailsComponent, data: { title: 'Email' } },
	{ path: ':filter', component: ConversationsComponent, canActivate: [ConverationsGuard] },
	{ path: ':filter/:id', component: ConversationsComponent, data: { title: 'Conversations' } },
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
