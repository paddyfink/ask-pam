import { Component, AfterViewInit } from '@angular/core';

import { TdMediaService, IPageChangeEvent, TdLoadingService } from '@covalent/core';
import { Router } from '@angular/router';

@Component({
    selector: 'settings-overview',
    templateUrl: './settings-overview.component.html',
    styleUrls: ['./settings-overview.component.scss'],
})

export class SettingsOverviewComponent implements AfterViewInit {

    public event: IPageChangeEvent;

    constructor(
        public media: TdMediaService,
        private _loadingService: TdLoadingService,
        private router: Router
    ) {
    }

    ngOnInit() {
    }

    ngAfterViewInit(): void {
        // broadcast to all listener observables when loading the page
        this.media.broadcast();
    }
}