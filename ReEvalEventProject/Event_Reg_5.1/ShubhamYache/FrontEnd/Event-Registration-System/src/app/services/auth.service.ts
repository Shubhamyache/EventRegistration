import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { map } from 'rxjs/operators';
import { catchError } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { User } from '../models/user.model';
import { RegUser } from '../models/regUser.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class UserAuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  private roleSubject = new BehaviorSubject<string>('');
  private usernameSubject = new BehaviorSubject<string>('');

  isLoggedIn$ = this.isLoggedInSubject.asObservable();
  userRole$ = this.roleSubject.asObservable();
  username$ = this.usernameSubject.asObservable();

  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient, private route: Router) {
    // const token = localStorage.getItem('token');
    // if (token) {
    //   this.isLoggedInSubject.next(true);
    // }
  }

  login(email: string, password: string) {
    return this.http
      .post<any>(`${environment.apiUrl}/Authentication/login`, {
        Email: email,
        Password: password,
      })
      .pipe(
        map((response) => {
          if (response && response.token) {
            localStorage.setItem('token', response.token);
            this.isLoggedInSubject.next(true);
            this.roleSubject.next(response.Role);
            this.usernameSubject.next(response.Username); // Set username if available
            return response;
          }
          return null;
        })
      );
  }

  checkUserNameExists(username: string): Observable<boolean> {
    return this.http
      .get<any>(
        `${this.baseUrl}/Authentication/checkUserNameExists?email=${username}`
      )
      .pipe(
        map((response) => {
          if (response && response.message === 'User already exists') {
            return true;
          } else {
            return false;
          }
        })
      );
  }

  addUser(user: RegUser): Observable<any> {
    return this.http.post(`${this.baseUrl}/Authentication/register/user`, user);
  }

  addOrganizer(user: RegUser): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/Authentication/register/organizer`,
      user
    );
  }
  logout() {
    localStorage.removeItem('token');
    this.isLoggedInSubject.next(false);
    this.roleSubject.next('');
    this.usernameSubject.next('');
    this.route.navigate(['/home']);
  }

  getAllUsers(): Observable<any> {
    return this.http.get(`${this.baseUrl}/User`);
  }
}
