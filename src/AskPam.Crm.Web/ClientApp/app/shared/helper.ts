import { Injectable } from '@angular/core';
import { ErrorInfo } from './errorInfo';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';

@Injectable()
export class Helper {
	constructor(public snackBar: MatSnackBar) {

	}

	HandleError(error: any): void {
		let errorInfo = JSON.parse(error.response);
		this.snackBar.open(errorInfo.message, '', {
			duration: 3000,
		});
	}

	displaySnackbar(message: string, action?: string): void {
		let config = new MatSnackBarConfig();
		config.duration = 3000;
		// config.viewContainerRef = this.viewContainerRef;
		this.snackBar.open(message, action, config);
	}

	displayError(message: string): void {
		let config = new MatSnackBarConfig();
		config.duration = 5000;
		// config.viewContainerRef = this.viewContainerRef;
		this.snackBar.open(message, 'Close', config);
	}
}

// this.snackBarComponent.openFromComponent(SnackBarComponent, config);