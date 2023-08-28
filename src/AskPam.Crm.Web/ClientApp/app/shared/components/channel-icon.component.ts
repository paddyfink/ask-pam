import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'app-channel-icon',
    template: `
    <i class="fa " [ngClass]="{
        'fa-envelope':type === 'Email' && open === false,
        'fa-envelope-open':type === 'Email' && open === true,
        'fa-comment':type === 'Sms',
        'fa-globe':type === 'Web',
        'fa-facebook':type === 'Facebook',
        'fa-phone-square':type === 'Viber',
        'fa-weixin':type === 'WeChat',
        'fa-comments-o':type === 'Line',
        'fa-telegram':type === 'Telegram',
        'error':hasError
        }">
    </i>
        `,
    styles: [`
    .error {
        color: #ff5252;
    }
    `]
})
export class ChannelIconComponent implements OnInit {

    @Input() type = '';
    @Input() open = false;
    @Input() hasError = false;
    constructor() { }

    ngOnInit() { }
}
