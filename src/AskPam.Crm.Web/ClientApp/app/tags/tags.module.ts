import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatChipsModule} from '@angular/material';

import { TagsComponent } from './tags.component';
import { TagsService, TagDto } from '../services/crm.services';
import { CovalentChipsModule } from '@covalent/core';

@NgModule({
	imports: [
		CommonModule,
		CovalentChipsModule,
		FormsModule, ReactiveFormsModule,
		MatChipsModule
	],
	declarations: [
		TagsComponent
	],
	providers: [TagsService],
	exports: [TagsComponent]
})
export class TagsModule { }
