import { Event } from './event.model';
import { Payment } from './payment.model';
import { User } from './user.model';

export interface Registration {
  registrationId: number;
  userId: string;
  user?: User;
  eventId: number;
  event?: Event;
  registrationDateTime: string;
  platinumTicketsCount: number;
  goldTicketsCount: number;
  silverTicketsCount: number;
  approveDate?: Date | null;
  payments?: Payment[] | null;
  paymentDto?: Payment[] | null;
  registrationStatus?: string;
}

export interface RegistrationModel {
  registrationDto: Registration;
  paymentDto: Payment;
}
