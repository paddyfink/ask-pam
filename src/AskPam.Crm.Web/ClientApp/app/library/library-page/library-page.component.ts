import {
    Component,
    AfterViewInit,
    ChangeDetectorRef,
    OnInit
} from '@angular/core';

import { LibraryService, LibraryItemListDto, LibraryItemTypeDto } from '../../services/crm.services';

import {
    TdMediaService,
    IPageChangeEvent,
    TdDialogService,
    TdDataTableSortingOrder
} from '@covalent/core';

import { Helper } from '../../shared/helper';

@Component({
    selector: 'app-library-page',
    templateUrl: './library-page.component.html',
    providers: [LibraryService]
})

export class LibraryPageComponent implements OnInit, AfterViewInit {

    event: IPageChangeEvent;

    filteredData: LibraryItemListDto[];
    types: LibraryItemTypeDto[];
    filtered = false;

    // Search
    searchTerm = '';

    // Pagination
    filteredTotal = 0;
    fromRow = 1;
    currentPage = 1;
    pageSize = 20;

    // Filter
    libraryItemTypeId?: number = null;

    // Sorting
    columnOptions: any[] = [
        { name: 'Name', value: 'name' },
        { name: 'Date', value: 'createdAt' },
    ];
    sortKey: string = this.columnOptions[0].value;
    headers: { [key: string]: TdDataTableSortingOrder } = {};

    constructor(
        public media: TdMediaService,
        private helper: Helper,
        private libraryService: LibraryService,
        private _dialogService: TdDialogService,
        private _changeDetectorRef: ChangeDetectorRef
    ) {
    }

    loadHeaders(): void {
        this.columnOptions.forEach((option: any) => {
            this.headers[option.value] = TdDataTableSortingOrder.Ascending;
        });
    }

    search(searchTerm: string): void {
        this.searchTerm = searchTerm;
        this.filter();
    }

    page(pagingEvent: IPageChangeEvent): void {
        this.fromRow = pagingEvent.fromRow;
        this.currentPage = pagingEvent.page;
        this.pageSize = pagingEvent.pageSize;
        this.filter();
    }

    sortBy(sortKey: string): void {
        if (this.headers[sortKey] === TdDataTableSortingOrder.Ascending) {
            this.headers[sortKey] = TdDataTableSortingOrder.Descending;
        } else {
            this.headers[sortKey] = TdDataTableSortingOrder.Ascending;
        }
        this.sortKey = sortKey;
        this.filter();
    }

    filterByType(libraryItemTypeId: number) {
        this.libraryItemTypeId = libraryItemTypeId;
        this.filter();
    }

    getLibraryItemTypes(): void {

        this.libraryService.getLibraryItemTypes()
            .subscribe(result => {
                this.types = result;
            }, error => {
                this.helper.HandleError(error);
            });
    }

    filter(): void {

        let sorting = this.sortKey;
        if (this.headers[this.sortKey] === TdDataTableSortingOrder.Descending) {
            sorting += ' DESC';
        }
        this.libraryService.getLibraryItems({
            filter: this.searchTerm,
            sorting: sorting,
            maxResultCount: this.pageSize,
            skipCount: this.fromRow - 1,
            libraryTypeId: this.libraryItemTypeId
        })
            .subscribe(result => {
                this.filteredData = result.items;
                this.filteredTotal = result.totalCount;
                this.filtered = true;
            }, error => {
                this.helper.HandleError(error);
            });
    }

    delete(id: number): void {
        this._dialogService
            .openConfirm({ message: 'Are you sure you want to delete this library?' })
            .afterClosed().subscribe((confirm: boolean) => {
                if (confirm) {
                    this.libraryService.deleteLibraryItem(id)
                        .subscribe(() => {
                            this.filter();
                            this.helper.displaySnackbar('Library deleted successfully.');
                        }, error => {
                            this.helper.HandleError(error);
                        });
                }
            });
    }

    ngAfterViewInit(): void {
        // broadcast to all listener observables when loading the page
        this.media.broadcast();
        this._changeDetectorRef.detectChanges();
    }

    ngOnInit() {
        this.loadHeaders();
        this.getLibraryItemTypes();
        // this.filter();
    }
}
