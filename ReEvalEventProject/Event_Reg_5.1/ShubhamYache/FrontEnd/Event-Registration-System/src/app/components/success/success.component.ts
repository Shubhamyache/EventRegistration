import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrationService } from '../../services/registration.service';
import { saveAs } from 'file-saver'; // npm install file-saver
import { CommonModule, DatePipe } from '@angular/common';
import { CommonEngine } from '@angular/ssr';

@Component({
  standalone: true,
  imports: [DatePipe, CommonModule],
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.css'],
})
export class SuccessComponent implements OnInit {
  userId!: string;
  eventId!: string;
  registrationDetails: any;
  ticketNumber!: string;

  constructor(
    private route: ActivatedRoute,
    private registrationService: RegistrationService
  ) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userId')!;
    this.eventId = this.route.snapshot.paramMap.get('eventId')!;
    this.fetchRegistrationDetails();
  }

  fetchRegistrationDetails() {
    this.registrationService
      .getRegistrationsByUserId(this.userId)
      .subscribe((registrations) => {
        console.log('Registration details:', registrations);

        // Find the registration details matching the eventId
        this.registrationDetails = registrations.find(
          (reg: { eventId: number }) => reg.eventId === +this.eventId
        );
        console.log('Registration details:', this.registrationDetails);

        if (this.registrationDetails) {
          // Create ticket number in format T+registrationId+EventId
          this.ticketNumber = `T${this.registrationDetails.registrationId}${this.eventId}`;
        }
      });
  }

  downloadTicket() {
    const ticketContent = `
      Event ID: ${this.registrationDetails.eventId}
      Registration Date: ${this.registrationDetails.registrationDateTime}
      Ticket Number: ${this.ticketNumber}
      Platinum Tickets: ${this.registrationDetails.platinumTicketsCount}
      Gold Tickets: ${this.registrationDetails.goldTicketsCount}
      Silver Tickets: ${this.registrationDetails.silverTicketsCount}
      Status: ${this.registrationDetails.registrationStatus}
    `;

    const blob = new Blob([ticketContent], {
      type: 'text/plain;charset=utf-8',
    });
    saveAs(blob, `${this.ticketNumber}-ticket.txt`);
  }
}
