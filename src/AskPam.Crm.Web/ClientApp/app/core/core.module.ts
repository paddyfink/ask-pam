import { NotificationsEffects } from '../redux/notifications/notifications.effects';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from '../services/authInterceptor';
import { appRoutingProviders } from '../app.routes';
// import { AUTH_PROVIDERS } from 'angular2-jwt';
import { Auth } from '../services/auth.service';
import {
  AccountService, ConversationsService, ContactsService, UsersService,
  NotificationsService, SettingsService
} from '../services/crm.services';
import { Helper } from '../shared/helper';
import { InAppNotificationService } from '../shared/inAppNotification.service';
import { EffectsModule } from '@ngrx/effects';
import { UserEffects } from '../redux/users/users.effects';
import { ContactsEffects } from '../contacts/state/contacts.effects';
import { SessionEffects } from '../redux/session/effects';
import { HttpErrorInterceptor } from '../services/httpErrorInterceptor';
import { ConversationEffects } from '../conversations/state/conversations.effects';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    EffectsModule.forRoot([ConversationEffects, UserEffects, ContactsEffects, SessionEffects, NotificationsEffects])],
  exports: [],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: HttpErrorInterceptor,
    //   multi: true,
    // },
    appRoutingProviders,
    //  AUTH_PROVIDERS,
    Auth,
    AccountService,
    Helper,
    InAppNotificationService,
    ConversationsService,
    ContactsService,
    UsersService,
    NotificationsService,
    SettingsService
    // { provide: RouterStateSerializer, useClass: CustomRouterStateSerializer }
  ],
})
export class CoreModule { }
