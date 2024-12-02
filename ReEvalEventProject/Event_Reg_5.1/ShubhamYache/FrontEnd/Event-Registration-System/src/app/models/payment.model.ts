import { Registration } from './registration.modal';

export interface Payment {
  paymentId: number;
  registrationId: number;
  paymentDateTime?: string;
  paymentAmount: number;
  paymentMethod: string;
  paymentStatus: string;
  typeOfTransaction: string;
}
