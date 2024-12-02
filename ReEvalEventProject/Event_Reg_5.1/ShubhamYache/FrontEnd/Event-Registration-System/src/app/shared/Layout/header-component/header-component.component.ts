import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { UserAuthService } from '../../../services/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-header-component',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './header-component.component.html',
  styleUrl: './header-component.component.css',
})
export class HeaderComponentComponent implements OnInit {
  isLoggedIn$: Observable<boolean> = new Observable<boolean>();
  userRole$: Observable<string> = new Observable<string>();
  username: string = '';
  constructor(private authService: UserAuthService) {}

  ngOnInit() {
    this.isLoggedIn$ = this.authService.isLoggedIn$;
    this.userRole$ = this.authService.userRole$;
    this.username =
      `${sessionStorage.getItem('firstName')} ${sessionStorage.getItem(
        'lastName'
      )}` || '';
  }

  logout() {
    this.authService.logout();
  }
}
