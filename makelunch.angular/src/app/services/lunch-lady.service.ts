import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Restaurant } from '../models/restaurant';
import { HttpService } from './http.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LunchLadyService {

  constructor(private http: HttpService) { }

  public getRestaurant(): Observable<Restaurant> {
    let url = `/api/restaurants/`;
		return this.http.get(url).pipe(map(res => <Restaurant>res));
  }
}
