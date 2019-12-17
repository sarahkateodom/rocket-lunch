import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Restaurant } from '../models/restaurant';
import { HttpService } from './http.service';
import { map } from 'rxjs/operators';
import { SocialLogin } from '../models/social-login';
import { UUID } from 'angular2-uuid';
import { RestaurantSearch } from '../models/restaurant-search';
import { SocialUser, SocialLoginModule } from 'angularx-social-login';
import { User } from '../models/user';
import { GeocodeResult } from '../models/geocode-result';

@Injectable({
  providedIn: 'root'
})
export class LunchLadyService {

  constructor(private http: HttpService) { }

  public login(socialUser: SocialUser): Observable<any> {
    let url = `/api/login`;
    return this.http.post(url, { googleId: socialUser.id, name: socialUser.name, email: socialUser.email, photoUrl: socialUser.photoUrl } as SocialLogin).pipe(map(res => res));
  }

  public logout(): Observable<any> {
    let url = `/api/logout`;
    return this.http.get(url).pipe(map(res => res));
  }

  public getPosition(): Promise<any> {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(resp => {
        resolve({
          lng: resp.coords.longitude, lat: resp.coords.latitude
        });
      },
        err => {
          reject(err);
        });
    });
  }

  public getGeocodeResult(lng: string, lat: string): Observable<GeocodeResult> {
    let url = `https://api.mapbox.com/geocoding/v5/mapbox.places/${lng}%2C%20${lat}.json?access_token=pk.eyJ1Ijoib3hmb3JkbGFic2NsZ3giLCJhIjoiY2s0MXc5c2lpMDU3eTNvcDlleGYzZXVwNSJ9.1WoZ9Vkzpu8dzztCZYT53g`;
    return this.http.get(url).pipe(map(res => <GeocodeResult>res));
  }

  public getRestaurant(guid: UUID, searchModel: RestaurantSearch): Observable<Restaurant> {
    let url = `/api/restaurants/${guid}`;
    return this.http.post(url, searchModel).pipe(map(res => <Restaurant>res));
  }

  public getRestaurants(zip: string): Observable<Restaurant[]> {
    let url = `/api/restaurants/${zip}`;
    return this.http.get(url).pipe(map(res => <Restaurant[]>res));
  }

  public getCurrentUser(): Observable<User> {
    let url = `/api/users/current`;
    return this.http.get(url).pipe(map(res => <User>res));
  }

  public getUser(id: number): Observable<User> {
    let url = `/api/users/${id}`;
    return this.http.get(url).pipe(map(res => <User>res));
  }

  public updateuser(user: User): Observable<boolean> {
    let url = `/api/users/${user.id}`;
    return this.http.put(url, user).pipe(map(res => <boolean>res));
  }

  // public createUserSession(users: number[]): Observable<UUID> {
  //   let url = `/api/sessions/`;
  //   return this.http.post(url, users).pipe(map(res => <UUID>res));
  // }

  // public updateUserSession(id: UUID, users: number[]): Observable<boolean> {
  //   let url = `/api/sessions/${id}`;
  //   return this.http.put(url, users).pipe(map(res => <boolean>res));
  // }
}
