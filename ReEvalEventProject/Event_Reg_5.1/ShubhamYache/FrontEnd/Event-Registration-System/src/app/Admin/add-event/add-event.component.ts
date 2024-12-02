import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { EventService } from '../../services/event.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-createevent',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  templateUrl: './add-event.component.html',
})
export class AddEventComponent implements OnInit {
  eventForm!: FormGroup;
  venues: any[] = [];
  selectedVenue?: any;
  categories = [
    'Technology',
    'Arts',
    'Community',
    'Entertainment',
    'Learning',
    'Music',
    'Sports',
    'Travel',
  ];

  // Properties for warnings
  eventNameWarn: string = '';
  categoryWarn: string = '';
  eventStartDateTimeWarn: string = '';
  eventEndDateTimeWarn: string = '';
  registrationCloseDateWarn: string = '';
  minimumAgeWarn: string = '';
  platinumTicketsNumberWarn: string = '';
  platinumTicketsPriceWarn: string = '';
  goldTicketsNumberWarn: string = '';
  goldTicketsPriceWarn: string = '';
  silverTicketsNumberWarn: string = '';
  silverTicketsPriceWarn: string = '';
  hashtagWarn: string = '';
  descriptionWarn: string = '';
  venueIdWarn: string = '';
  totalTicketsWarn: string = '';
  imageUrlWarn: string = '';

  constructor(
    private fb: FormBuilder,
    private eventService: EventService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.eventForm = this.fb.group({
      eventName: ['', [Validators.required, Validators.minLength(3)]],
      category: ['', Validators.required],
      eventStartDateTime: ['', [Validators.required, this.futureDateValidator]],
      eventEndDateTime: [
        '',
        [
          Validators.required,
          this.futureDateValidator,
          this.endDateAfterStartDateValidator,
        ],
      ],
      registrationCloseDate: [
        '',
        [
          Validators.required,
          this.futureDateValidator,
          this.registrationCloseDateValidator,
        ],
      ],
      minimumAge: [
        0,
        [Validators.required, Validators.min(0), Validators.max(21)],
      ],
      platinumTicketsNumber: [0, [Validators.required, Validators.min(0)]],
      platinumTicketsPrice: [0, [Validators.required, Validators.min(0)]],
      goldTicketsNumber: [0, [Validators.required, Validators.min(0)]],
      goldTicketsPrice: [0, [Validators.required, Validators.min(0)]],
      silverTicketsNumber: [0, [Validators.required, Validators.min(0)]],
      silverTicketsPrice: [0, [Validators.required, Validators.min(0)]],
      hashtag: ['', Validators.minLength(3)],
      description: ['', [Validators.required, Validators.minLength(10)]],
      venueId: ['', Validators.required],
      imageUrl: ['', Validators.required],
    });
    this.getVenues();
  }

  getVenues(): void {
    this.eventService.getVenues().subscribe((venues: any[]) => {
      this.venues = venues;
    });
  }

  futureDateValidator(
    control: AbstractControl
  ): { [key: string]: boolean } | null {
    const currentDate = new Date();
    const inputDate = new Date(control.value);
    return inputDate > currentDate ? null : { futureDate: true };
  }

  endDateAfterStartDateValidator = (
    control: AbstractControl
  ): { [key: string]: boolean } | null => {
    const startDate = new Date(
      this.eventForm?.get('eventStartDateTime')?.value
    );
    const endDate = new Date(control.value);
    return endDate > startDate ? null : { endDateBeforeStartDate: true };
  };

  registrationCloseDateValidator = (
    control: AbstractControl
  ): { [key: string]: boolean } | null => {
    const startDate = new Date(
      this.eventForm?.get('eventStartDateTime')?.value
    );
    const closeDate = new Date(control.value);
    return closeDate < startDate
      ? null
      : { registrationCloseDateAfterStartDate: true };
  };

  onSubmit(): void {
    // Reset warnings
    this.resetAllWarnings();

    let formValidity = true;
    const controls = this.eventForm.controls;

    this.selectedVenue = this.venues.find(
      (ven) => ven.venueId == parseInt(this.eventForm.get('venueId')?.value)
    );

    const startDateTimeStr = controls['eventStartDateTime'].value;
    const endDateTimeStr = controls['eventEndDateTime'].value;
    const registrationCloseDateStr = controls['registrationCloseDate'].value;

    const currentDate = new Date();
    const startDateTime = new Date(startDateTimeStr);
    const endDateTime = new Date(endDateTimeStr);
    const registrationCloseDateTime = new Date(registrationCloseDateStr);

    if (startDateTime <= currentDate) {
      this.eventStartDateTimeWarn =
        'Please select a future date and time for the event start.';
      formValidity = false;
    } else {
      this.eventStartDateTimeWarn = '';
    }

    const minimumDurationMs = 30 * 60 * 1000; // 30 minutes in milliseconds
    if (
      endDateTime <= startDateTime ||
      endDateTime.getTime() - startDateTime.getTime() < minimumDurationMs
    ) {
      this.eventEndDateTimeWarn =
        'Please select a future date and time for the event end, at least 30 minutes after the start time.';
      formValidity = false;
    } else {
      this.eventEndDateTimeWarn = '';
    }

    // Check if the registration close date is before the event start date
    if (registrationCloseDateTime >= startDateTime) {
      this.registrationCloseDateWarn =
        'Registration close date must be before the event start date.';
      formValidity = false;
    } else {
      this.registrationCloseDateWarn = '';
    }

    const totalTickets =
      controls['platinumTicketsNumber'].value +
      controls['goldTicketsNumber'].value +
      controls['silverTicketsNumber'].value;

    if (totalTickets < 10) {
      this.totalTicketsWarn =
        'The total number of tickets must be at least 10.';
      formValidity = false;
    }

    if (this.selectedVenue != undefined) {
      if (totalTickets > this.selectedVenue!.maxCapacity) {
        this.totalTicketsWarn =
          'The total number of tickets must be less than Venue capacity ' +
          this.selectedVenue!.maxCapacity;
        formValidity = false;
      }
    }

    if (this.selectedVenue == undefined) {
      this.venueIdWarn = 'Please select a venue.';
      formValidity = false;
    }

    if (controls['eventName'].invalid) {
      this.eventNameWarn =
        'Please enter a valid event name with at least 3 characters.';
      formValidity = false;
    }

    if (controls['category'].invalid) {
      this.categoryWarn = 'Please select a category.';
      formValidity = false;
    }

    if (controls['description'].invalid) {
      this.descriptionWarn =
        'Please enter a valid description with at least 10 characters.';
      formValidity = false;
    }

    if (controls['venueId'].invalid) {
      this.venueIdWarn = 'Please select a venue.';
      formValidity = false;
    }

    if (controls['eventStartDateTime'].invalid) {
      this.eventStartDateTimeWarn =
        'Please select a future date and time for the event start.';
      formValidity = false;
    }

    if (controls['eventEndDateTime'].invalid) {
      this.eventEndDateTimeWarn =
        'Please select a future date and time for the event end, after the start time.';
      formValidity = false;
    }

    if (controls['registrationCloseDate'].invalid) {
      this.registrationCloseDateWarn =
        'Registration close before the event start date.';
      formValidity = false;
    }

    if (controls['minimumAge'].invalid) {
      this.minimumAgeWarn =
        'Please enter a valid minimum age between 0 and 21.';
      formValidity = false;
    }

    if (controls['imageUrl'].invalid) {
      this.imageUrlWarn = 'Please enter a valid image URL.';
      formValidity = false;
    }

    // if (controls['platinumTicketsNumber'].invalid) {
    //   this.platinumTicketsNumberWarn = 'Please enter a valid number of platinum tickets.';
    //   formValidity = false;
    // }

    // if (controls['platinumTicketsPrice'].invalid) {
    //   this.platinumTicketsPriceWarn = 'Please enter a valid price for platinum tickets.';
    //   formValidity = false;
    // }

    // if (controls['goldTicketsNumber'].invalid) {
    //   this.goldTicketsNumberWarn = 'Please enter a valid number of gold tickets.';
    //   formValidity = false;
    // }

    // if (controls['goldTicketsPrice'].invalid) {
    //   this.goldTicketsPriceWarn = 'Please enter a valid price for gold tickets.';
    //   formValidity = false;
    // }

    // if (controls['silverTicketsNumber'].invalid) {
    //   this.silverTicketsNumberWarn = 'Please enter a valid number of silver tickets.';
    //   formValidity = false;
    // }

    // if (controls['silverTicketsPrice'].invalid) {
    //   this.silverTicketsPriceWarn = 'Please enter a valid price for silver tickets.';
    //   formValidity = false;
    // }

    if (formValidity) {
      console.log('All fields valid. Submitting form data.');
      const loggedOrganizerEmail = sessionStorage.getItem('loggedEmail') || '';
      if (loggedOrganizerEmail == '') {
        console.log('userid not found');
        return;
      }
      const formValues = {
        ...this.eventForm.value,
      };

      var event: any = {
        eventId: 0,
        eventName: formValues.eventName,
        category: formValues.category,
        eventStartDateTime: startDateTime.toISOString(),
        eventEndDateTime: endDateTime.toISOString(),
        registrationCloseDate: registrationCloseDateTime.toISOString(),
        minimumAge: formValues.minimumAge,
        platinumTicketsNumber: formValues.platinumTicketsNumber,
        platinumTicketsPrice: formValues.platinumTicketsPrice,
        goldTicketsNumber: formValues.goldTicketsNumber,
        goldTicketsPrice: formValues.goldTicketsPrice,
        silverTicketsNumber: formValues.silverTicketsNumber,
        silverTicketsPrice: formValues.silverTicketsPrice,
        eventStatus: 'Published',
        hashtag:
          formValues.hashtag != '' ? formValues.hashtag : formValues.eventName,
        description: formValues.description,
        venueId: parseInt(formValues.venueId),
        organizerEmail: loggedOrganizerEmail,
        imageUrl: formValues.imageUrl,
      };

      console.log(event);

      this.eventService.createEvent(event).subscribe((event: any) => {
        console.log('################');
        console.log(event);
        Swal.fire('Success', 'Event added successfully!', 'success').then(
          () => {
            this.router.navigate(['/admin/admin-dashboard']);
          }
        );
      });
    } else {
      console.log('All fields not valid.');
    }
  }

  resetAllWarnings() {
    this.eventNameWarn = '';
    this.categoryWarn = '';
    this.eventStartDateTimeWarn = '';
    this.eventEndDateTimeWarn = '';
    this.registrationCloseDateWarn = '';
    this.minimumAgeWarn = '';
    this.platinumTicketsNumberWarn = '';
    this.platinumTicketsPriceWarn = '';
    this.goldTicketsNumberWarn = '';
    this.goldTicketsPriceWarn = '';
    this.silverTicketsNumberWarn = '';
    this.silverTicketsPriceWarn = '';
    this.hashtagWarn = '';
    this.descriptionWarn = '';
    this.venueIdWarn = '';
    this.totalTicketsWarn = '';
    this.imageUrlWarn = '';
  }

  resetWarnings(propertyName: string) {
    (this as any)[propertyName] = '';
  }
}
