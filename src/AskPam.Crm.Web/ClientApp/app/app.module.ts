import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { MatNativeDateModule } from '@angular/material';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { SimpleNotificationsModule } from 'angular2-notifications';

import { AppComponent } from './app.component';
import { routing } from './app.routes';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HotelsComponent } from './components/hotels/hotels.component';
import { MainComponent } from './main/main.component';
import { CoreModule } from './core/core.module';
import { metaReducers, reducers } from './redux';
import { SharedModule } from './shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import * as fromUsers from './redux/users';
import { ReactiveFormsModule } from '@angular/forms';
import { AgmCoreModule } from '@agm/core';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    // Dashboard
    DashboardComponent,

    // hotels
    HotelsComponent,
  ],
  entryComponents: [
    HotelsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatNativeDateModule,
    CoreModule,
    SharedModule,
    ReactiveFormsModule,

    routing,
    StoreModule.forRoot(reducers, { metaReducers }),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyDryMp8YmgIe6-m8_iP7_DqWhprXsQG0Sk'
    }),
    StoreModule.forFeature('users', fromUsers.reducers),
    // StoreRouterConnectingModule,
    StoreDevtoolsModule.instrument({ maxAge: 50 }),
    SimpleNotificationsModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
