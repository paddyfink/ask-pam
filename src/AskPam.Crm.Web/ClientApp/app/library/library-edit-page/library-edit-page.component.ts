import { Component, AfterViewInit, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { LibraryService, LibraryItemDto, LibraryItemTypeDto } from '../../services/crm.services';

import { TdMediaService, IPageChangeEvent, TdLoadingService } from '@covalent/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Helper } from '../../shared/helper';

@Component({
    selector: 'library-edit-page',
    templateUrl: './library-edit-page.component.html',
    styleUrls: ['./library-edit-page.component.scss'],
    providers: [LibraryService]
})

export class LibraryEditPageComponent implements AfterViewInit, OnInit {

    public event: IPageChangeEvent;

    public library: LibraryItemDto = <LibraryItemDto>{};
    types: LibraryItemTypeDto[];
    title: string = "Add a new library";

    public constructor(
        public media: TdMediaService,
        private _loadingService: TdLoadingService,
        private libraryService: LibraryService,
        private helper: Helper,
        private router: Router,
        private route: ActivatedRoute,
        private location: Location,
    ) {
    }

    expandedEvent(): void {
        //...
    }

    collapsedEvent(): void {
        //...
    }

    cancel(): void {
        this.location.back();
    }



    submit(): void {
        this._loadingService.register('library.edit');

        if (this.library.id) {
            this.updateLibrary();
        } else {
            this.createLibrary();
        }
    }

    createLibrary(): void {
        this._loadingService.register('library.edit');


        this.libraryService.createLibraryItem(this.library)
            .subscribe(result => {
                this._loadingService.resolve('library.edit');
                this.helper.displaySnackbar("Library added successfully.");
                this.router.navigate(['/library/view/', result.id]);
            }, error => {
                this.helper.displayError(error.message);
                this._loadingService.resolve('library.edit');
            });
    }

    updateLibrary(): void {
        this._loadingService.register('library.edit');

        this.libraryService.updateLibraryItem(this.library.id, this.library)
            .subscribe(result => {
                this._loadingService.resolve('library.edit');
                this.helper.displaySnackbar("Library updated successfully.");
                this.location.back();
            }, error => {
                this.helper.HandleError(error);
                this._loadingService.resolve('library.edit');
            });
    }

    getLibraryItemDetail(): void {
        this._loadingService.register('library.edit');

        this.libraryService.getLibraryItem(this.library.id)
            .subscribe(result => {
                this.library = result;
                this._loadingService.resolve('library.edit');
            }, error => {
                this.helper.HandleError(error);
                this._loadingService.resolve('library.edit');
            });
    }

    getLibraryItemTypes(): void {
        this._loadingService.register('library.edit');

        this.libraryService.getLibraryItemTypes()
            .subscribe(result => {
                this.types = result;
                this._loadingService.resolve('library.edit');
            }, error => {
                this.helper.HandleError(error);
                this._loadingService.resolve('library.edit');
            });
    }

    ngOnInit() {
        this.getLibraryItemTypes();

        this.route.params.subscribe((params: { id: number }) => {
            this.library.id = +params.id;
            if (this.library.id) {
                this.getLibraryItemDetail();
            }
        }
        );
    }

    ngAfterViewInit(): void {
        this.media.broadcast();
    }
}