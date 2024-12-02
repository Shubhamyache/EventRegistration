import {
  HttpClient,
  HttpClientModule,
  provideHttpClient,
  withFetch,
} from '@angular/common/http';
import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { EventService } from './services/event.service';
import { RegistrationService } from './services/registration.service';
import { HeaderComponentComponent } from './shared/Layout/header-component/header-component.component';
import { FooterComponentComponent } from './shared/Layout/footer-component/footer-component.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    HttpClientModule,
    RouterLink,
    HeaderComponentComponent,
    FooterComponentComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [EventService, RegistrationService],
})
export class AppComponent {
  title = 'Music-Event-Registration';
}
