import { Injectable } from '@angular/core';
import { tokenNotExpired } from 'angular2-jwt';
import { Router } from '@angular/router';
import { AccountService, AuthInfoDto, ProfileDto, } from './crm.services';
import { Const } from '../app.consts';
import { Observable } from 'rxjs/Observable';
// ReSharper disable once UnusedLocalImport
import { WebAuth } from 'auth0-js';
import { Store } from '@ngrx/store';
import { AppState } from '../redux';
import { OrganizationDto } from './crm.services';
import { ResetConversation } from '../conversations/state/conversation.actions';
import { ResetConversationList } from '../conversations/state/conversations-lists.actions';

@Injectable()
export class Auth {

  // Store profile object in auth class
  userAuthInfo: AuthInfoDto;
  roles: string[];

  constructor(private store: Store<AppState>,
    private router: Router,
    private accountService: AccountService) {
  }

  public login(username: string, password: string): Observable<void> {

    return Observable.create(observer => {
      this.accountService.login({ email: username, password: password })
        .subscribe(result => {
          localStorage.setItem(Const.idToken, result.idToken);
          localStorage.setItem(Const.organizationId, result.organizationId);
          this.userAuthInfo = result;
          observer.next();
        },
          error => observer.error(error));
    });

  }

  public getUserOrganization(): string {
    return localStorage.getItem(Const.organizationId);
  }
  public setUserOrganization(organizationId: string): void {
    localStorage.setItem(Const.organizationId, organizationId);
  }

  public authenticated() {
    // Check if there's an unexpired JWT
    // It searches for an item in localStorage with key == 'id_token'
    return tokenNotExpired('id_token');
  }

  public getUserRoles(): Observable<string[]> {
    return this.store.select(state => state.session.roles);

  }

  // public getUserInfo(): ProfileDto {
  //     let profile;
  //     this.store.select(state => state.session.profile).take(1).subscribe(profile => {
  //         profile = profile;
  //     });

  //     return profile
  // }

  public isAdmin$(): Observable<boolean> {
    return this.store.select(state => state.session.roles).map(roles => {
      return roles.indexOf(Const.userRoles.admin) > -1;
    });

  }

  public isHost$(): Observable<boolean> {
    return this.store.select(state => state.session.roles).map(roles => {
      return roles.indexOf(Const.userRoles.host) > -1;
    });
  }

  public switchOrganization(organization: OrganizationDto): void {

    this.setUserOrganization(organization.id);


    window.location.href = window.location.origin;
    // this.router.navigate(['/']);
    // window.location.reload();

    this.accountService.setDefaultOrgnization(organization.id)
      .subscribe(() => { });
  }

  public logout() {
    // Remove token from localStorage
    localStorage.removeItem(Const.organizationId);
    localStorage.removeItem(Const.idToken);
    this.userAuthInfo = null;
    this.store.dispatch(new ResetConversation());
    this.store.dispatch(new ResetConversationList());
    this.router.navigate(['/auth/login']);
  }

}
