import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';

import { TdMediaService, IPageChangeEvent } from '@covalent/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss'],
})

export class SettingsComponent implements AfterViewInit {

    public event: IPageChangeEvent;

    constructor(
        public media: TdMediaService,
        private router: Router,
        private changeDetectorRef: ChangeDetectorRef
    ) {
    }


    ngAfterViewInit(): void {
        // broadcast to all listener observables when loading the page
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
}