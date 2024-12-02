import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [RouterLink],
  selector: 'app-main-admin-dashboard',
  templateUrl: './main-admin-dashboard.component.html',
  styleUrl: './main-admin-dashboard.component.css',
})
export class MainAdminDashboardComponent {}
