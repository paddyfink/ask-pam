import { Component, Input, SimpleChanges, OnChanges, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { TdLoadingService } from '@covalent/core';
import { Helper } from '../shared/helper';

import { NotesService, NoteDto, ProfileDto } from '../services/crm.services';
import { Auth } from '../services/auth.service';
import { Store } from '@ngrx/store';
import { AppState } from '../redux/index';

@Component({
	selector: 'app-internal-notes',
	templateUrl: './internal-notes.component.html',
	styles: [`
	pre{
		white-space: pre-wrap;
	}
	`]
})
export class InternalNotesComponent implements OnInit, OnChanges {

	@Input() contactId: number;
	@Input() postId: number;
	@Input() placeHolder = 'add note';
	notes: NoteDto[] = [];
	comment: string;
	currentUser: ProfileDto;

	constructor(
		private _loadingService: TdLoadingService,
		private auth: Auth,
		private notesService: NotesService,
		private helper: Helper,
		private store: Store<AppState>
	) {
		this.store.select(state => state.session.profile).subscribe(profile => {
			this.currentUser = profile;
		});
	}

	ngOnChanges(changes: SimpleChanges) {
		let contactId = changes['contactId'];
		if (contactId && (contactId.previousValue !== contactId.currentValue)) {
			this.contactId = contactId.currentValue;
			this.Load();
		}
	}

	submit(form: FormGroup): void {
		// TMP, currently we dont delete so we can do that
		let note = <NoteDto>{
			createdById: this.currentUser.id,
			createdByFullName: this.currentUser.firstName + ' ' + this.currentUser.lastName,
			createdByicture: this.currentUser.picture,
			createdAt: new Date(),
			comment: this.comment
		};

		this.notes.push(note);

		this.notesService.createNote({
			comment: this.comment,
			contactId: this.contactId,
			postId: this.postId

		})
			.subscribe(result => {
			}, error => {
				this.helper.HandleError(error);
			});

		form.reset();
	}

	Load(): void {
		this.notesService.getNotes({ contactId: this.contactId, postId: this.postId })
			.subscribe(result => {
				this.notes = result;
			}, error => {
				this.helper.displayError(error.message);
			});
	}

	ngOnInit(): void {
		// this.Load();
	}
}
