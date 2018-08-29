import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Observable, Subscriber } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Injectable()
export class HttpService {
	constructor(private http: HttpClient) { 
	}

	private handleError: ((err: any, errorMsg?: string) => Observable<Response>) =
	((err, errMsg) => {
		if (err.status === 401 || err.status === 403) {

			// redirect you to forbidden
			//this.router.navigate(['forbidden']);

			return new Observable<Response>((subscriber: Subscriber<Response>) => {
				subscriber.complete();
			});
		} else if (err.status === 404) {
			// this.router.navigate(['notfound']);

			return new Observable<Response>((subscriber: Subscriber<Response>) => {
				subscriber.error(err);
			});
		} else {
			if (errMsg) {
				console.log(errMsg);
				//this.toastr.error(errMsg, 'Error');
			}

			return new Observable<Response>((subscriber: Subscriber<Response>) => {
				subscriber.error(err);
			});
		}
	}).bind(this);

	public get(url: string, errorMsg?: string) {
		return this.http.get(url, { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	public post(url: string, data: any, errorMsg?: string) {
		return this.http.post(url, JSON.stringify(data), { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	public put(url: string, data: any, errorMsg?: string) {
		return this.http.put(url, JSON.stringify(data), { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	public patch(url: string, data: any, errorMsg?: string) {
		return this.http.patch(url, JSON.stringify(data), { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	public delete(url: string, errorMsg?: string) {
		return this.http.delete(url, { headers: this.JSONHeaders() })
		.pipe(
			catchError(err => this.handleError(err, errorMsg))
		);
	}

	private JSONHeaders(): HttpHeaders {
		var headers = new HttpHeaders({
			'Content-Type': 'application/json',
			Accept: 'application/json',
			//'Authorization': 'Bearer ' + this.getJwt(),
		});

		// let jwt = this.getJwt();
		// if (jwt) {
		// 	headers.set('Authorization', 'Bearer ' + jwt);
		// }
		return headers;
	}
}