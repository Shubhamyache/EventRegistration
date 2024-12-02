import { Component } from '@angular/core';
import { EventListComponent } from '../../components/event-list/event-list.component';
import { Router } from '@angular/router';
import { RegistrationComponent } from '../../components/registration/registration.component';
import { RegistrationListComponent } from '../registration-list/registration-list.component';

@Component({
  standalone: true,
  imports: [EventListComponent, RegistrationListComponent],
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrl: './user-dashboard.component.css',
})
export class UserDashboardComponent {
  constructor(private router: Router) {}
  username: string = '';

  ngOnInit() {
    this.username =
      `${sessionStorage.getItem('firstName')} ${sessionStorage.getItem(
        'lastName'
      )}` || '';
  }

  exploreEvents() {
    this.router.navigate(['/events']);
  }
}
