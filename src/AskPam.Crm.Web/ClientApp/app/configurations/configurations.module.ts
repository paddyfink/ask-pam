import { SharedModule } from '../shared/shared.module';
import { NgModule } from '@angular/core';
import { ConfigurationsComponent } from './configurations.component';
import { SettingsService, OrganizationService } from '../services/crm.services';
import { FormsModule } from '@angular/forms';
import { routing } from './configurations.routing';

@NgModule({
    declarations: [
        ConfigurationsComponent
    ],
    imports: [
        SharedModule,
        routing],
    exports: [],
    providers: [SettingsService,OrganizationService],
})
export class ConfigurationsModule { }