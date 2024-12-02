import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/event.model';
import { BrowserModule } from '@angular/platform-browser';
import { SearchComponent } from '../search/search.component';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HeaderComponentComponent } from '../../shared/Layout/header-component/header-component.component';
import { FooterComponentComponent } from '../../shared/Layout/footer-component/footer-component.component';
import Swal from 'sweetalert2';

@Component({
  standalone: true,
  imports: [
    SearchComponent,
    RouterLink,
    CommonModule,
    HeaderComponentComponent,
    FooterComponentComponent,
  ],
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss'],
  providers: [],
})
export class EventListComponent implements OnInit {
  events: Event[] = []; // Array to store fetched events

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadEvents(); // Fetch events on component initialization
  }

  // Method to fetch events from the service
  loadEvents(): void {
    this.eventService.getActiveEvents().subscribe(
      (data: Event[]) => {
        this.events = data; // Assign the fetched data to the events array
      },
      (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Oops...',
          text: 'Network Error! Please try again later!',
        });
        console.error('Error fetching events:', error);
      }
    );
  }
}
