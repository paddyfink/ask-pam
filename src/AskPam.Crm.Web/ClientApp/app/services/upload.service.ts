// import { ServiceBase } from './crm.services.extension';

// import 'rxjs/Rx';
// import { Observable } from 'rxjs/Observable';
// import { Injectable, Inject, Optional, OpaqueToken } from '@angular/core';
// import { Http, Headers, Response } from '@angular/http';
// import { ProfilePictureDto, SwaggerException } from './crm.services';

// export const API_BASE_URL = new OpaqueToken('API_BASE_URL');

// @Injectable()
// export class UploadService extends ServiceBase {
//     private http: Http = null;
//     private baseUrl: string | undefined = undefined;
//     protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

//     constructor( @Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
//         super();
//         this.http = http;
//         this.baseUrl = baseUrl ? baseUrl : "";
//     }

//     uploadProfilePicture(file: File): Observable<ProfilePictureDto> {
//         let url_ = this.baseUrl + "/api/upload/UploadProfilePicture";
//         url_ = url_.replace(/[?&]$/, "");

//         const content_ = new FormData();
//         if (file !== null)
//             content_.append("file", file);

//         let options_ = {
//             body: content_,
//             method: "post",
//             headers: new Headers({
//                 "Accept": "application/json; charset=UTF-8"
//             })
//         };

//         return Observable.fromPromise(this.transformOptions(options_)).flatMap(transformedOptions_ => {
//             return this.http.request(url_, transformedOptions_);
//         }).map((response) => {
//             return this.transformResult(url_, response, (response) => this.processUploadProfilePicture(response));
//         }).catch((response: any) => {
//             if (response instanceof Response) {
//                 try {
//                     return Observable.of(this.transformResult(url_, response, (response) => this.processUploadProfilePicture(response)));
//                 } catch (e) {
//                     return <Observable<ProfilePictureDto>><any>Observable.throw(e);
//                 }
//             } else
//                 return <Observable<ProfilePictureDto>><any>Observable.throw(response);
//         });
//     }



//     protected processUploadProfilePicture(response: Response): Observable<ProfilePictureDto> {
//         const responseText = response.text();
//         const status = response.status;

//         if (status === 200) {
//             let result200: ProfilePictureDto | null = null;
//             result200 = responseText === "" ? null : <ProfilePictureDto>JSON.parse(responseText, this.jsonParseReviver);
//             return Observable.of(result200);
//         } else if (status !== 200 && status !== 204) {
//             this.throwException("An unexpected server error occurred.", status, responseText);
//         }
//         return null;
//     }

//     resetProfilePicture(): Observable<ProfilePictureDto> {
//         let url_ = this.baseUrl + "/api/upload/ResetProfilePicture";
//         url_ = url_.replace(/[?&]$/, "");

//         const content_ = "";

//         let options_ = {
//             body: content_,
//             method: "post",
//             headers: new Headers({
//                 "Content-Type": "application/json; charset=UTF-8",
//                 "Accept": "application/json; charset=UTF-8"
//             })
//         };

//         return Observable.fromPromise(this.transformOptions(options_)).flatMap(transformedOptions_ => {
//             return this.http.request(url_, transformedOptions_);
//         }).map((response) => {
//             return this.transformResult(url_, response, (response) => this.processResetProfilePicture(response));
//         }).catch((response: any) => {
//             if (response instanceof Response) {
//                 try {
//                     return Observable.of(this.transformResult(url_, response, (response) => this.processResetProfilePicture(response)));
//                 } catch (e) {
//                     return <Observable<ProfilePictureDto>><any>Observable.throw(e);
//                 }
//             } else
//                 return <Observable<ProfilePictureDto>><any>Observable.throw(response);
//         });
//     }

//     protected processResetProfilePicture(response: Response): ProfilePictureDto {
//         const responseText = response.text();
//         const status = response.status;

//         if (status === 200) {
//             let result200: ProfilePictureDto | null = null;
//             result200 = responseText === "" ? null : <ProfilePictureDto>JSON.parse(responseText, this.jsonParseReviver);
//             return result200;
//         } else if (status !== 200 && status !== 204) {
//             this.throwException("An unexpected server error occurred.", status, responseText);
//         }
//         return null;
//     }

//     protected throwException(message: string, status: number, response: string, result?: any): any {
//         if (result !== null && result !== undefined)
//             throw result;
//         else
//             throw new SwaggerException(message, status, response, null);
//     }
// }