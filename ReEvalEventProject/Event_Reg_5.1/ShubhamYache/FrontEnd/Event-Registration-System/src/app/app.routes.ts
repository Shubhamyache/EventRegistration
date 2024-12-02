import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EventListComponent } from './components/event-list/event-list.component';
import { EventDetailsComponent } from './components/event-details/event-details.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { SuccessComponent } from './components/success/success.component';
import { SearchComponent } from './components/search/search.component';
import { LoginComponent } from './shared/login/login.component';
import { SignupComponent } from './shared/sign-up/sign-up.component';
import { LandingPageComponent } from './shared/landing-page/landing-page.component';
import { AuthGuard } from './auth.guard';
import { UserDashboardComponent } from './User/user-dashboard/user-dashboard.component';
import { AdminDashboardComponent } from './Admin/admin-dashboard/admin-dashboard.component';
import { AddEventComponent } from './Admin/add-event/add-event.component';
import { ViewEventComponent } from './Admin/view-event/view-event.component';
import { MainAdminDashboardComponent } from './mainAdmin/main-admin-dashboard/main-admin-dashboard.component';
import { AllUsersListComponent } from './mainAdmin/all-users-list/all-users-list.component';
import { AllEventsListComponent } from './mainAdmin/all-events-list/all-events-list.component';
export const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'home', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  {
    path: 'registration/:id',
    component: RegistrationComponent,
    canActivate: [AuthGuard],
  },
  // { path: 'success/:id', component: SuccessComponent },
  { path: 'events', component: EventListComponent },
  { path: 'search', component: SearchComponent },
  {
    path: 'success/:userId/:eventId',
    component: SuccessComponent,
    canActivate: [AuthGuard],
  },

  {
    path: 'event-details/:id',
    component: EventDetailsComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'user-dashboard',
    component: UserDashboardComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin-dashboard',
    component: AdminDashboardComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/add-event',
    component: AddEventComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin/view-events',
    component: ViewEventComponent,
    canActivate: [AuthGuard],
  },
  { path: 'main-admin', component: MainAdminDashboardComponent },
  { path: 'main-admin/view-events', component: AllEventsListComponent },
  { path: 'main-admin/view-users', component: AllUsersListComponent },
  { path: '**', redirectTo: '/home' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
