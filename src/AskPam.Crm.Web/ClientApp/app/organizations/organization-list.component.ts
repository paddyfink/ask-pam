import { Component, OnInit, AfterViewInit } from '@angular/core';

import { TdMediaService } from '@covalent/core';
import { OrganizationService, OrganizationDto } from '../services/crm.services';
import { MatDialog } from '@angular/material';
import { OrganizationAddComponent } from './organization-add.component';

@Component({
	selector: 'organization-list',
	templateUrl: './organization-list.component.html',
	//styleUrls: ['./conversation.component.scss'],
	providers: [OrganizationService]
})
export class OrganizationListComponent implements OnInit, AfterViewInit {

	organizations: OrganizationDto[] = [];

	constructor(private media: TdMediaService,
		private organizationService: OrganizationService,
		public dialog: MatDialog, ) {
	}

	ngAfterViewInit(): void {
		// broadcast to all listener observables when loading the page
		this.media.broadcast();
	}

	ngOnInit() {
		this.organizationService.getOrganizations({ maxResultCount: 100, skipCount: 0, sorting: '' }).subscribe(result => {
			this.organizations = result.items;
		});
	}

	openDialog() {
		let dialogRef = this.dialog.open(OrganizationAddComponent);
		dialogRef.afterClosed().subscribe(organization => {
			if (organization) {
				this.organizations.push(organization);
			}
		});
	}

}
