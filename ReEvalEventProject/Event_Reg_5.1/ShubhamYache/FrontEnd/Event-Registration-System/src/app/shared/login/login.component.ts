import { Component, Inject, inject, OnInit, PLATFORM_ID } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { UserAuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { decode } from 'node:punycode';
import { ToastrService } from 'ngx-toastr';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  constructor(
    private fb: FormBuilder,

    private router: Router,
    private authService: UserAuthService,
    private toastr: ToastrService
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngOnInit(): void {}

  onSubmit() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value; //here username is email
      console.log('In onsubmit' + username + password);

      // Call the authentication service to login
      this.authService.login(username, password).subscribe(
        (response) => {
          console.log('in response', response);

          // Get the JWT token from the response
          const token = response.token; // Assuming the response contains the token
          localStorage.setItem('token', token); // Save the token in local storage

          // Decode JWT token to get user information
          const decodedToken: any = jwtDecode(token);
          const role = decodedToken.Role;
          localStorage.setItem('role', role); // Save the role in local storage
          const firstName = decodedToken.FirstName;
          sessionStorage.setItem('firstName', firstName); // Save the first name in session storage
          const lastName = decodedToken.LastName;
          sessionStorage.setItem('lastName', lastName); // Save the last name in session storage
          const email = decodedToken.email;
          sessionStorage.setItem('loggedEmail', email); // Save the email in session storage
          const userId = decodedToken.UserId;
          sessionStorage.setItem('userId', userId); // Save the user ID in session storage

          //debug
          console.log('Role:', role); // Log the role
          console.log('First Name:', firstName); // Log the first name
          console.log('Last Name:', lastName); // Log the last name
          console.log('Email:', email); // Log the email
          console.log('Decoded Token:', decodedToken); // Log the decoded token

          // Navigate to appropriate dashboard based on role
          if (role === 'User') {
            this.toastr.success('Login Successful', 'Welcome to MusicNook!');
            this.router.navigate(['user-dashboard']);
          } else if (role === 'Organizer') {
            Swal.fire('Login', 'Welcome Organizer!', 'success').then(() => {
              this.router.navigate(['admin-dashboard']);
            });
          } else if (role === 'Admin') {
            Swal.fire('Login', 'Welcome Admin!', 'success').then(() => {
              this.router.navigate(['main-admin']);
            });
          }
        },
        (error) => {
          if (error.status === 0) {
            Swal.fire('Network Error', 'Check the Connection', 'warning');
          } else {
            // Handle other errors such as invalid login credentials
            this.errorMessage = 'Invalid login credentials. Please try again.';
            Swal.fire('Error', this.errorMessage, 'error');
          }
        }
      );
    }
  }
}
