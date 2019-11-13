import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Restaurant } from '../models/restaurant';
import { HttpService } from './http.service';
import { map } from 'rxjs/operators';
import { User } from '../models/user';
import { UUID } from 'angular2-uuid';
import { MealTime } from '../models/enums/MealTime';

@Injectable({
  providedIn: 'root'
})
export class LunchLadyService {

  constructor(private http: HttpService) { }

  public login(googleId: string, name: string, email: string): Observable<any> {
    let url = `/api/login`;
    return this.http.post(url, { googleId: googleId, name: name, email: email }).pipe(map(res => res));
  }

  public logout(): Observable<any> {
    let url = `/api/logout`;
    return this.http.get(url).pipe(map(res => res));
  }

  public getRestaurant(guid: UUID): Observable<Restaurant> {
    let url = `/api/restaurants/${guid}`;
    return this.http.get(url).pipe(map(res => <Restaurant>res));
  }

  public getRestaurantByTime(guid: UUID, meal: MealTime): Observable<Restaurant> {
    let url = `/api/restaurants/${guid}/${meal}`;
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

  public getUser(id: number): Observable<User> {
    let url = `/api/users/${id}`;
    return this.http.get(url).pipe(map(res => <User>res));
  }

  public updateuser(user: User): Observable<boolean> {
    let url = `/api/users/`;
    return this.http.put(url, user).pipe(map(res => <boolean>res));
  }

  public createUserSession(users: number[]): Observable<UUID> {
    let url = `/api/sessions/`;
    return this.http.post(url, users).pipe(map(res => <UUID>res));
  }

  public updateUserSession(id: UUID, users: number[]): Observable<boolean> {
    let url = `/api/sessions/${id}`;
    return this.http.put(url, users).pipe(map(res => <boolean>res));
  }
}
