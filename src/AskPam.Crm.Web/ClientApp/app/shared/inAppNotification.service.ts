import { Injectable } from '@angular/core';
import { ErrorInfo } from './errorInfo';
import { NotificationsService as Notifier } from 'angular2-notifications';

enum Type {
    Info,
    Warning,
    Succes,
    Error
}

@Injectable()
export class InAppNotificationService {

    constructor(private notifier: Notifier) {
    }

    ApiError(errorInfo: ErrorInfo) {

        this.notifier.error('Error', errorInfo.message, {
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: true,
            clickToClose: true,
        });
    }

    error(message: string, title?: string) {
    }

    success(message: string, title: string) {
        this.notifier.success(title, message, {
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: true,
            clickToClose: true
        });
    }

    info(message: string, title?: string) {
        this.notifier.info(title, message, {
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: true,
            clickToClose: true,
            position: ['top', 'right']
        });
    }

}
