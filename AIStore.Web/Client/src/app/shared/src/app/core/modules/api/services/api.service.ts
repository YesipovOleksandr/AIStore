import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import Helpers from "../../oauth/helpers/helpers";

@Injectable()
export class ApiService {
  constructor(private http: HttpClient) {
  }

  public get<T>(baseUrl: string, resource: string, accessToken: string = null): Observable<T> {
    const headers = {};

    if (accessToken) {
      headers['Authorization'] = "Bearer " + accessToken;
    }

    //headers['Access-Control-Allow-Origin'] = '*';
    //headers['Content-Type'] = 'application/json';
    //headers['Content-Type'] = 'application/x-www-form-urlencoded; charset=UTF-8';


    return this.http.get<T>(`${baseUrl}${resource}`, { headers, withCredentials: false });
  }

  public postFormData(baseUrl: string, resource: string, formData: FormData, headers: {} = {}): Observable<any> {
    var url = `${baseUrl}${resource}`;
    return this.http.post<any>(
      url,
      formData,
      {
        headers,
        responseType: 'text' as 'json',
        reportProgress: true,
        observe: "events",
        withCredentials: true
      }
    );
  }

  public post<T>(baseUrl: string, resource: string, body: {}, asJson = true, headers: {} = {}, withCredential = true, accessToken: string = null): Observable<T> {
    const params = Helpers.MapParams(body, asJson);

    if (asJson) {
      headers['Content-Type'] = 'application/json; charset=utf-8';
    } else {
      headers['Content-Type'] = 'application/x-www-form-urlencoded';
    }

    if (accessToken) {
      headers['Authorization'] = "Bearer " + accessToken;
    }

    var url = `${baseUrl}${resource}`;
    return this.http.post<T>(url, params, { headers, withCredentials: withCredential });
  }
}
