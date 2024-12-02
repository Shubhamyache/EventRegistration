import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { environment } from '../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  private baseUrl = environment.apiUrl + '/Registration';

  constructor(private http: HttpClient) {}
  addRegistration(registrationData: any): Observable<any> {
    registrationData.approveDate = new Date().toISOString();
    return this.http.post(`${this.baseUrl}/AddRegistration`, registrationData);
  }

  getRegistrationsByUserId(userId: string): Observable<any> {
    return this.http.get(
      `${this.baseUrl}/GetRegistrationsByUserId?userId=${userId}`
    );
  }

  cancelRegistration(registrationId: number): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/CancelRegistration?id=${registrationId}`,
      {}
    );
  }
}
