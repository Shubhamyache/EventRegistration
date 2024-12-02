import { Component, NgModule, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/event.model';
import { RegistrationService } from '../../services/registration.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';

declare var bootstrap: any; // Add this line to declare the 'bootstrap' variable

@Component({
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css'],
})
export class EventDetailsComponent implements OnInit {
  event: any = {}; // Store event details
  registrationForm!: FormGroup;
  paymentForm!: FormGroup;
  totalAmount: number = 0;
  showPaymentModal: boolean = false;
  userId: string = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private registrationService: RegistrationService
  ) {}

  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('id');
    this.userId = sessionStorage.getItem('userId') || '';
    this.getEventDetails(eventId!);

    // Initialize registration form
    this.registrationForm = this.fb.group({
      platinumTicketCount: [0, [Validators.min(0), Validators.max(10)]],
      goldTicketCount: [0, [Validators.min(0), Validators.max(10)]],
      silverTicketCount: [0, [Validators.min(0), Validators.max(10)]],
    });

    // Initialize payment form
    this.paymentForm = this.fb.group({
      paymentMethod: ['', Validators.required],
      upiId: [''],
      cardNumber: [''],
      expiryDate: [''],
      cvv: [''],
    });

    // Calculate total amount whenever form values change
    this.registrationForm.valueChanges.subscribe(() => {
      this.calculateTotalAmount();
    });

    // Adjust payment form based on selected payment method
    this.paymentForm.get('paymentMethod')?.valueChanges.subscribe((method) => {
      if (method === 'UPI') {
        this.paymentForm.get('upiId')?.setValidators([Validators.required]);
        this.paymentForm.get('cardNumber')?.clearValidators();
        this.paymentForm.get('expiryDate')?.clearValidators();
        this.paymentForm.get('cvv')?.clearValidators();
      } else {
        this.paymentForm.get('upiId')?.clearValidators();
        this.paymentForm
          .get('cardNumber')
          ?.setValidators([Validators.required]);
        this.paymentForm
          .get('expiryDate')
          ?.setValidators([Validators.required]);
        this.paymentForm.get('cvv')?.setValidators([Validators.required]);
      }
      this.paymentForm.get('upiId')?.updateValueAndValidity();
      this.paymentForm.get('cardNumber')?.updateValueAndValidity();
      this.paymentForm.get('expiryDate')?.updateValueAndValidity();
      this.paymentForm.get('cvv')?.updateValueAndValidity();
    });
  }

  // Fetch event details from API
  getEventDetails(eventId: string) {
    this.http
      .get(`https://localhost:7234/api/Event/GetEventById?eventId=${eventId}`)
      .subscribe((data: any) => {
        this.event = data;
        this.calculateTotalAmount(); // Recalculate once event details are fetched
      });
  }

  // Calculate the total ticket price based on selected counts
  calculateTotalAmount() {
    const { platinumTicketCount, goldTicketCount, silverTicketCount } =
      this.registrationForm.value;
    this.totalAmount =
      platinumTicketCount * this.event.platinumTicketsPrice +
      goldTicketCount * this.event.goldTicketsPrice +
      silverTicketCount * this.event.silverTicketsPrice;
  }

  // Open payment modal
  openPaymentModal() {
    this.showPaymentModal = true;
  }

  // Close payment modal
  closePaymentModal() {
    this.showPaymentModal = false;
  }

  // Submit the registration and payment
  onSubmit() {
    if (this.registrationForm.invalid || this.paymentForm.invalid) return;

    const registrationData = {
      userId: this.userId,
      eventId: this.event.eventId,
      registrationDateTime: new Date().toISOString(),
      registrationStatus: 'Completed',
      platinumTicketsCount: this.registrationForm.get('platinumTicketCount')
        ?.value,
      goldTicketsCount: this.registrationForm.get('goldTicketCount')?.value,
      silverTicketsCount: this.registrationForm.get('silverTicketCount')?.value,
      approveDate: null,
      paymentDto: {
        paymentAmount: this.totalAmount,
        paymentMethod: this.paymentForm.get('paymentMethod')?.value,
        paymentDateTime: new Date().toISOString(),
        paymentStatus: 'Completed',
        typeOfTransaction: 'Online',
      },
    };

    // Debugging: Logging the registration data before sending the request
    console.log('Sending registration data:', registrationData);

    // Call the service to handle the registration API call
    this.registrationService.addRegistration(registrationData).subscribe(
      (response: any) => {
        // Check if the response status is OK (200) or contains a success message
        if (
          response === 'Registration Successfull!' ||
          response.status === 200
        ) {
          Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Registration successful',
          }).then(() => {
            this.router.navigate([
              `/success/${this.userId}/${this.event.eventId}`,
            ]);
          });
        } else {
          // If something unexpected happens, show a generic error
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'An unexpected error occurred. Please try again.',
          });
          console.error('Unexpected response:', response);
        }
      },
      (error) => {
        // Handle specific error responses from the backend
        if (error.status === 404) {
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Event not found',
          });
          console.error('Event not found');
        } else if (error.status === 400) {
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: error.error || 'Over-subscribed or payment mismatch',
          });
          console.error('Bad Request:', error.error); // Over-subscribed or payment mismatch message
        } else {
          // General error handling
          Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Registration successful',
          }).then(() => {
            this.router.navigate([
              `/success/${this.userId}/${this.event.eventId}`,
            ]);
          });
        }
      }
    );
  }
}
