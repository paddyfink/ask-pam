import {
  AfterViewInit,
  Component,
  NgZone,
  OnInit,
  ViewChild
} from "@angular/core";

import {
  MatDialog,
  MatDrawerToggleResult,
  MatSidenav
} from "@angular/material";
import { Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";
import { AppState } from "../redux";
import { GetSession } from "../redux/session/actions";
import { LoadUsers } from "../redux/users/users.actions";
import { Auth } from "../services/";
import {
  AccountService,
  GetNotificationsRequestDto,
  NotificationDto,
  NotificationsService,
  OrganizationDto,
  ProfileDto,
  SettingsService
} from "../services/crm.services";
import { NotificationHubService } from "../services/SignalR/NotificationHubService";
import { Helper } from "../shared/helper";
import { TdMediaService } from "@covalent/core";
import {
  LoadNotifications,
  GetUnreadCount
} from "../redux/notifications/notifications.actions";
import { ResetConversation } from "../conversations/state/conversation.actions";
import { LoadConversationFilters } from "../conversations/state/conversations-filters.action";
import { ResetConversationList } from "../conversations/state/conversations-lists.actions";
import { interval } from "rxjs/observable/interval";

@Component({
  providers: [Auth, NotificationHubService],
  selector: "app-main",
  styleUrls: ["./main.component.scss"],
  templateUrl: "./main.component.html"
})
export class MainComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSidenav) public sidenav: MatSidenav;
  public layout$: Observable<any>;
  public user: ProfileDto;
  public organizationId: string;
  public organizationName: string;

  public isAdmin$: Observable<boolean>;
  public isHost$: Observable<boolean>;

  public intercomAppId: string;

  public filtersLoaded: Observable<boolean>;
  public filterId: Observable<number>;

  public organizations: OrganizationDto[] = [];

  constructor(
    public media: TdMediaService,
    private auth: Auth,
    private store: Store<AppState>,
    private notificationHub: NotificationHubService,
    private router: Router,
    private zone: NgZone,
    private helper: Helper,
    private _settingsServ: SettingsService,
    private accountService: AccountService,
    private dialog: MatDialog
  ) {
    this.layout$ = this.store.select(state => state.layout);
    this.organizationId = this.auth.getUserOrganization();
    this.isAdmin$ = this.auth.isAdmin$();
    this.isHost$ = this.auth.isHost$();
    this.store
      .select(state => state.session.organizations)
      .subscribe(result => {
        this.organizations = result.filter(o => o.id !== this.organizationId);
        const currentOrg = result.find(o => o.id === this.organizationId);
        if (currentOrg) {
          this.organizationName = currentOrg.name;
        }
      });

    this.store.select(state => state.session.profile).subscribe(user => {
      if (user.id) {
        this.user = user;

        this._settingsServ.getAppSettings().subscribe(settings => {
          this.intercomAppId = settings.intercomAppId;

          (window as any).Intercom("boot", {
            app_id: this.intercomAppId,
            user_id: user.id,
            name: user.firstName + " " + user.lastName,
            email: user.email,
            // created_at: 1312182000
            custom_launcher_selector: "#intecomLauncher"
          });

          // const self = this;
          // (window as any).Intercom("onUnreadCountChange", function (unreadCount) {
          // 	self.zone.run(() => {
          // 		self.intercomUnreadCount = unreadCount;
          // 	});
          // });
        });
      }
    });
  }

  public logout(): void {
    this.auth.logout();
  }

  public switchOrganization(organization: OrganizationDto): void {
    if (this.organizationId !== organization.id) {
      this.auth.switchOrganization(organization);
    }
  }

  // public loadFeatures(): void {
  // 	this.organizationService.getFeatures(this.auth.getUserOrganization())
  // 		.subscribe(result => {
  // 			this.features = result;
  // 		});
  // }

  ngOnInit() {
    this.store.dispatch(new GetSession({}));
    this.store.dispatch(new LoadUsers());
    this.store.dispatch(
      new LoadNotifications({ maxResultCount: 20, skipCount: 0 })
    );
    this.store.dispatch(new GetUnreadCount({}));

    interval(600000).subscribe(i => {
      this.store.dispatch(
        new LoadNotifications({ maxResultCount: 20, skipCount: 0 })
      );
      this.store.dispatch(new GetUnreadCount({}));
    });
    this.notificationHub.init();
  }

  ngAfterViewInit() {}
}
