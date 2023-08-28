import { Component, Input, Output, EventEmitter, AfterViewInit, forwardRef } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../redux';
import { Helper } from '../shared/helper';

import { TagsService, TagDto, TagRelationDto } from '../services/crm.services';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';



@Component({
	selector: 'app-tags',
	providers: [TagsService,
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => TagsComponent),
			multi: true
		}],
	templateUrl: './tags.component.html'
})

export class TagsComponent implements AfterViewInit, ControlValueAccessor {

	@Input() model: TagDto[];
	@Input() readOnly = false;
	@Input() appendToBody = true;
	@Output() modelChange = new EventEmitter();
	tags: TagDto[];
	filteredTags: TagDto[];

	@Input() contactId: number;
	@Input() libraryId: number;
	@Input() conversationId: number;
	@Input() messageId: number;
	@Input() autoSave = false;

	constructor(
		private store: Store<AppState>,
		private tagsService: TagsService,
		private helper: Helper,
	) {
	}

	propagateChange = (_: any) => { };

	writeValue(obj: any): void {
		if (obj !== undefined) {
			this.model = obj;
		}
	}
	registerOnChange(fn: any): void {
		this.propagateChange = fn;
	}
	registerOnTouched(fn: any): void {

	}
	setDisabledState?(isDisabled: boolean): void {
		throw new Error('Method not implemented.');
	}

	filterTagsList(value: string): void {
		this.filteredTags = null;
		if (value) {
			this.filteredTags = this.tags.filter((item: TagDto) => {
				return (item.name + item.category).toLowerCase().indexOf(value.toLowerCase()) > -1;
			}).filter((filteredItem: TagDto) => {
				return !this.model.find(modelItem => {
					return modelItem.id === filteredItem.id;
				});
			});
		}
	}

	onAdd(tag): void {
		this.modelChange.emit(this.model);
		this.propagateChange(this.model);

		if (this.autoSave) {
			this.tagsService.tag(<TagRelationDto>{
				tagId: tag.id,
				tagName: tag.name,
				contactId: this.contactId,
				libraryId: this.libraryId,
				conversationId: this.conversationId,
				messageId: this.messageId
			})
				.subscribe(result => {

				}, error => {
					this.helper.displayError(error.message);
				});
		}
	}

	onRemove(tag: ITag): void {
		this.modelChange.emit(this.model);
		this.propagateChange(this.model);
		if (this.autoSave) {
			this.tagsService.untag(<TagRelationDto>{
				tagId: tag.id,
				tagName: tag.name,
				contactId: this.contactId,
				libraryId: this.libraryId,
				conversationId: this.conversationId,
				messageId: this.messageId
			})
				.subscribe(result => {
				}, error => {
					this.helper.displayError(error.message);
				});
		}
	}

	// loadSelectedTags(request): void {
	// 	this.tagsService.getItemTags({ contactId: request.contactId, libraryId: request.libraryId })
	// 		.subscribe(result => {
	// 			this.model = result;
	// 		}, error => {
	// 			this.helper.displayError(error.message);
	// 		});
	// }

	loadAllTags(): void {
		this.tagsService.getAllTags()
			.subscribe(result => {
				this.tags = result;
			}, error => {
				this.helper.displayError(error.message);
			});
	}

	ngAfterViewInit(): void {
		if (!this.readOnly) {
			this.loadAllTags();
		}
	}
}

export interface ITag {
	id: number;
	name: string;
}
