import { Component, AfterViewInit, ViewChild, ChangeDetectorRef, OnInit } from '@angular/core';
import { MatDrawer, MatDrawerToggleResult } from '@angular/material';
import { TdMediaService, IPageChangeEvent } from '@covalent/core';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
})

export class ProfileComponent implements AfterViewInit, OnInit {

    public event: IPageChangeEvent;

    constructor(
        public media: TdMediaService,
        private changeDetectorRef: ChangeDetectorRef
    ) {
    }

    ngOnInit() {
    }

    ngAfterViewInit(): void {
        // broadcast to all listener observables when loading the page
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
}
