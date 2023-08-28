import { ITdDataTableColumn, TdDialogService, TdMediaService } from '@covalent/core';
import { Component, OnInit, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { SettingDto, SettingsService, OrganizationService, OrganizationDto } from '../services/crm.services';

@Component({
    selector: 'app-configurations',
    templateUrl: './configurations.component.html'
})
export class ConfigurationsComponent implements OnInit, AfterViewInit {


    organizationId: string;
    userId: string;
    settings: SettingDto[];
    organizations: OrganizationDto[];

    columns: ITdDataTableColumn[] = [
        { name: 'name', label: 'Name' },
        { name: 'value', label: 'Value' },
    ];

    constructor(
        public media: TdMediaService,
        private settingsService: SettingsService,
        private organizationService: OrganizationService,
        private _dialogService: TdDialogService,
        private changeDetectorRef: ChangeDetectorRef) {
    }

    loadSettings() {
        this.settingsService.getSettings({ organizationId: this.organizationId, userId: this.userId })
            .subscribe(result => {
                this.settings = result;
            });
    }

    openPrompt(row: any): void {
        this._dialogService.openPrompt({
            message: 'Enter value?',
            value: row['value'],
        }).afterClosed().subscribe((value: any) => {
            row['value'] = value;
            this.settingsService.updateSettings(row).subscribe();
        });
    }

    loadOrganizations() {
        this.organizationService.getOrganizations({ maxResultCount: 100, skipCount: 0, sorting: "" }).subscribe(result => {
            this.organizations = result.items
        })
    }

    ngAfterViewInit(): void {
        // broadcast to all listener observables when loading the page
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }

    ngOnInit() {
        this.loadSettings();
        this.loadOrganizations();
    }
}