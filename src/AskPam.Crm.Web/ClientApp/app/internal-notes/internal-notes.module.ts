import { SharedModule } from '../shared/shared.module';
import { NgModule } from '@angular/core';

//3rd Party
import { TagInputModule } from 'ngx-chips';

import { InternalNotesComponent } from './internal-notes.component';
import { NotesService, TagDto } from '../services/crm.services';


@NgModule({
	imports: [
		SharedModule,
		TagInputModule,
		
	],
	declarations: [
		InternalNotesComponent
	],
	providers: [NotesService],
	exports: [InternalNotesComponent]
})
export class InternalNotesModule { }