import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { EventData } from '../models/eventData.model';
import { map } from 'rxjs/operators';
import { catchError } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { Venue } from '../models/venue.model';
import { Event } from '../models/event.model';
@Injectable({
  providedIn: 'root',
})
export class EventService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getActiveEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.baseUrl}/Event/GetActiveEvents`);
  }

  getEventById(id: number): Observable<any> {
    const url = `${this.baseUrl}/Event/GetEventById?eventId=${id}`;
    return this.http.get<EventData>(url);
  }

  getVenues(): Observable<Venue[]> {
    return this.http.get<Venue[]>(`${this.baseUrl}/Venue/GetVenues`);
  }

  createEvent(event: EventData): Observable<EventData> {
    return this.http.post<EventData>(`${this.baseUrl}/Event/AddEvent`, event);
  }
  addEvent(eventData: EventData): Observable<any> {
    return this.http.post(`${this.baseUrl}/AddEvent`, eventData);
  }

  getAllEvents(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Event/GetAllEvents`);
  }

  // Method to update event tag
  updateEventTag(eventId: number, tag: string): Observable<any> {
    const url = `${
      this.baseUrl
    }/Event/updateEventTag?eventId=${eventId}&tag=${encodeURIComponent(tag)}`;
    return this.http.put(url, null); // `null` because no request body is required
  }

  updateEventStatus(eventId: number, status: string): Observable<any> {
    const url = `${this.baseUrl}/Event/updateEventStatus?eventId=${eventId}&status=${status}`;
    return this.http.put(url, null);
  }
}
