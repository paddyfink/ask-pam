import { Response } from '@angular/http';

export class ErrorInfo {
	constructor() {
		this.reset();
	}

	message: string;

	response: Response;
	errors: string[];

	reset() {
		this.message = '';
	}

	/**
	 * Displays an error alert
	 * @param msg  - Either a message string or error object with .message property
	 */
	error(msg) {
		if (typeof (msg) === 'object' && msg.message) {
			this.message = msg.message;
		} else {
			this.message = msg;
		}

	}


	/**
	 * Parse a toPromise() .catch() clause error
	 * from a response object and returns an errorInfo object
	 * @param response
	 * @returns {Promise<void>|Promise<T>}
	 */
	parsePromiseResponseError(response) {

		if (response.hasOwnProperty('message')) {
			return Promise.reject(response);
		}
		if (response.hasOwnProperty('Message')) {
			response.message = response.Message;
			return Promise.reject(response);
		}

		let err = new ErrorInfo();
		err.response = response;
		err.message = response.statusText;

		try {
			let data = response.json();
			if (data && data.message) {
				err.message = data.message;
				err.errors = data.errors;
			}
		} catch (ex) {

		}

		return Promise.reject(err);
	}

	parseObservableResponseError(response): any {

		if (response.hasOwnProperty('message')) {
			return response;
		}
		if (response.hasOwnProperty('Message')) {
			response.message = response.Message;
			return response;
		}

		let err = new ErrorInfo();
		err.response = response;
		err.message = response.statusText;

		try {
			let data = JSON.parse(response);
			if (data && data.message) {
				err.message = data.message;
			}
		} catch (ex) { }

		if (!err.message) {
			err.message = 'Unknown server failure.';
		}

		return err;
	}
}
