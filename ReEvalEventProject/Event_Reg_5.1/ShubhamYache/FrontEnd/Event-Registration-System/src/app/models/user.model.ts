import { Registration } from './registration.modal';

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  password?: string;
  registrations?: Registration[];
  registrationCount?: number;
  eventCount?: number;
}

export interface UserRegistration {
  firstname: string;
  lastname: string;
  email: string;
  phoneNumber: string;
  password: string;
}
