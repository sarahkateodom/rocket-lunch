import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Restaurant } from '../models/restaurant';
import { HttpService } from './http.service';
import { map } from 'rxjs/operators';
import { User } from '../models/user';
import { UUID } from 'angular2-uuid';

@Injectable({
  providedIn: 'root'
})
export class LunchLadyService {

  constructor(private http: HttpService) { }

  public getRestaurant(guid: UUID): Observable<Restaurant> {
    let url = `/api/restaurants/${guid}`;
    return this.http.get(url).pipe(map(res => <Restaurant>res));
  }

  public getRestaurants(): Observable<Restaurant[]> {
    let url = `/api/restaurants/`;
    return this.http.get(url).pipe(map(res => <Restaurant[]>res));
  }

  public addUser(user: User): Observable<number> {
    let url = `/api/users/`;
    return this.http.post(url, user).pipe(map(res => <number>res));
  }

  public getUsers(): Observable<User[]> {
    let url = `/api/users/`;
    return this.http.get(url).pipe(map(res => <User[]>res));
  }
}
