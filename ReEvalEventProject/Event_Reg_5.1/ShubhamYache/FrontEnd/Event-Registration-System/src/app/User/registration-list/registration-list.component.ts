import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';
import { EventService } from '../../services/event.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-registration-list',
  templateUrl: './registration-list.component.html',
  styleUrls: ['./registration-list.component.css'],
})
export class RegistrationListComponent implements OnInit {
  registrations: any[] = [];
  currentBookings: any[] = [];
  pastBookings: any[] = [];
  userId: string = '';

  constructor(
    private registrationService: RegistrationService,
    private eventService: EventService
  ) {}

  ngOnInit(): void {
    this.userId = sessionStorage.getItem('userId') || '';
    this.loadRegistrations();
  }

  loadRegistrations() {
    // Clear existing data to prevent appending
    this.currentBookings = [];
    this.pastBookings = [];

    if (this.userId) {
      this.registrationService.getRegistrationsByUserId(this.userId).subscribe({
        next: (data) => {
          this.registrations = data;
          const currentDate = new Date();

          this.registrations.forEach((registration) => {
            // Fetch event details and categorize registrations into current/past
            this.eventService
              .getEventById(registration.eventId)
              .subscribe((event) => {
                registration.eventDetails = event;
                const eventEndDate = new Date(event.eventEndDateTime);

                // Categorize into current or past bookings
                if (eventEndDate >= currentDate) {
                  this.currentBookings.push(registration);
                } else {
                  this.pastBookings.push(registration);
                }
              });
          });
        },
        error: (error) => {
          console.error('Failed to fetch registrations', error);
        },
      });
    }
  }

  cancelRegistration(registrationId: number) {
    Swal.fire({
      title: 'Are you sure?',
      text: 'Are you sure you want to cancel this registration?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, cancel it!',
      cancelButtonText: 'No, keep it',
    }).then((result) => {
      if (result.isConfirmed) {
        // Call the cancel registration service
        this.registrationService.cancelRegistration(registrationId).subscribe({
          next: () => {
            Swal.fire(
              'Cancelled!',
              'Your registration has been cancelled. Refund Initiated.',
              'success'
            );
            // Reload the bookings after successful cancellation
            this.loadRegistrations();
          },
          error: (error) => {
            Swal.fire(
              'Error!',
              'Failed to cancel registration. Please try again later.',
              'error'
            );
            console.error('Failed to cancel registration', error);
          },
        });
      }
    });
  }
}
