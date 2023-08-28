import { AccountService } from '../services';
import { LoginComponent, ForgetPasswordComponent } from './index';
import { routing } from './auth.routing';
import { SharedModule } from '../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [
        LoginComponent,
        ForgetPasswordComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        routing],
    exports: [],
    providers: [AccountService],
})
export class AuthModule { }