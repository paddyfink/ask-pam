import { NgModule, } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



import { FollowersComponent } from './followers.component';
import { FollowersService } from '../services/crm.services';
import { CovalentChipsModule } from '@covalent/core';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		CovalentChipsModule
	],
	declarations: [
		FollowersComponent
	],
	providers: [FollowersService],
	exports: [FollowersComponent]
})
export class FollowersModule { }
