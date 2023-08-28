import { Const } from '../app.consts';
import { ErrorInfo } from '../shared/errorInfo';

import { Observable } from 'rxjs/Observable'; // ignore
import { Response, RequestOptionsArgs } from '@angular/http'; // ignore

export class ServiceBase {

  protected transformOptions(options: RequestOptionsArgs) {

    var token = localStorage.getItem(Const.idToken);
    var organizationId = localStorage.getItem(Const.organizationId);

    if (token) {
      options.headers.append('Authorization', 'Bearer ' + token);
      options.headers.append('Organization', organizationId);
    }

    return Promise.resolve(options);
  }

  protected transformResult(url: string, response: Response, processor: (response: Response) => any): Observable<any> {
    if (response.status !== 200 && response.status !== 204) {
      var err = new ErrorInfo().parseObservableResponseError(response);
      throw err;

    }
    else {
      return processor(response);
    }
  }



}
