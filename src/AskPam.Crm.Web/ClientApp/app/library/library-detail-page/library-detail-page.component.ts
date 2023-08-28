import { Component, AfterViewInit, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { TdMediaService, IPageChangeEvent, TdLoadingService } from '@covalent/core';
import { ActivatedRoute } from '@angular/router';

import { LibraryService, LibraryItemDto } from '../../services/crm.services';
import { Helper } from '../../shared/helper';

@Component({
    selector: 'library-detail-page',
    templateUrl: './library-detail-page.component.html',
    styleUrls: ['./library-detail-page.component.scss'],
    providers: [LibraryService]
})

export class LibraryDetailPageComponent implements AfterViewInit, OnInit {
   
    event: IPageChangeEvent;
    library: LibraryItemDto = <LibraryItemDto>{};

    constructor(
        private media: TdMediaService,
        private _loadingService: TdLoadingService,
        private libraryService: LibraryService,
        private route: ActivatedRoute,
        private location: Location,
		private helper: Helper,
    ) {
    }

    cancel(): void {
        this.location.back();
    }

    getLibraryDetail(id: number): void {
        this._loadingService.register('overlay');
        this.libraryService.getLibraryItem(id)
            .subscribe(result => {
                this.library = result;
                this._loadingService.resolve('overlay');
            }, error => {
                this.helper.displayError(error.message);
                this._loadingService.resolve('overlay');
            });
    } 

    ngAfterViewInit(): void {
        // broadcast to all listener observables when loading the page
        this.media.broadcast();        
    }

    ngOnInit(): void {
        this.route.params.subscribe((params: { id: number }) => {
            if (params.id) {
                this.getLibraryDetail(params.id);
            }
        });
    }
}