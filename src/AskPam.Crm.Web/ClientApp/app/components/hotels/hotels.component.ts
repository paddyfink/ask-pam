import { Component, Inject } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
	selector: 'app-select-hotels',
	templateUrl: './hotels.component.html'
})

export class HotelsComponent {

	public eventName: string;
	private url: SafeResourceUrl;
	private href: string;

	constructor(@Inject(MAT_DIALOG_DATA) public data: any,
		private sanitizer: DomSanitizer
	) {
		this.eventName = this.data.eventName;
		this.url = this.sanitizer.bypassSecurityTrustResourceUrl('https://www.stay22.com/embed/' + this.eventName);
		this.href = 'https://www.stay22.com/events/' + this.eventName;
	}

}
