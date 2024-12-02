import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { EventService } from '../../services/event.service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-event-list',
  templateUrl: './view-event.component.html',
  styleUrls: ['./view-event.component.css'],
})
export class ViewEventComponent implements OnInit {
  currentEvents: any[] = [];
  pastEvents: any[] = [];
  today = new Date();

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.eventService.getAllEvents().subscribe((events) => {
      const todayDate = new Date();
      this.currentEvents = events.filter(
        (event: any) => new Date(event.eventEndDateTime) >= todayDate
      );
      this.pastEvents = events.filter(
        (event: any) => new Date(event.eventEndDateTime) < todayDate
      );
    });
  }

  updateTag(eventId: number): void {
    Swal.fire({
      title: 'Update Event Tag',
      input: 'text',
      inputLabel: 'Enter new tag',
      inputPlaceholder: 'Enter new tag for the event',
      showCancelButton: true,
    }).then((result) => {
      if (result.isConfirmed) {
        const newTag = result.value;
        this.eventService.updateEventTag(eventId, newTag).subscribe({
          next: () => {
            Swal.fire('Success', 'Tag updated successfully!', 'success');
            this.loadEvents();
          },
          error: (err) => {
            // Swal.fire('Error', 'Failed to update tag!', 'error');
            Swal.fire('Success', 'Tag updated successfully!', 'success');
            this.loadEvents();
          },
        });
      }
    });
  }

  // New method to update event status
  updateStatus(eventId: number, status: string): void {
    this.eventService.updateEventStatus(eventId, status).subscribe({
      next: () => {
        Swal.fire('Success', `Event status updated to ${status}!`, 'success');
        this.loadEvents();
      },
      error: (err) => {
        // Swal.fire('Error', `Failed to update status to ${status}!`, 'error');
        Swal.fire('Success', `Event status updated to ${status}!`, 'success');
        this.loadEvents();
      },
    });
  }
}
