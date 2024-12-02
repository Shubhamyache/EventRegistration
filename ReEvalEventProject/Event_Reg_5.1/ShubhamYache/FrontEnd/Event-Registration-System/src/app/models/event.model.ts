import { Registration } from './registration.modal';
import { User } from './user.model';
import { Venue } from './venue.model';

export interface Event {
  eventId: number;
  eventName: string;
  category: string;
  minimumAge: number; // Changed to match .NET model
  eventStartDateTime: string; // String is fine for handling DateTime in Angular
  eventEndDateTime: string;
  registrationCloseDate: string;
  platinumTicketsNumber: number;
  platinumTicketsPrice: number;
  goldTicketsNumber: number;
  goldTicketsPrice: number;
  silverTicketsNumber: number;
  silverTicketsPrice: number;
  eventStatus: string; // You can use a union type here for stricter typing
  hashtag: string; // Changed to match .NET model
  description: string; // Changed to match .NET model
  venueId: number;
  venue?: Venue | null;
  organizer?: User | null;
  organizerId: string;
  imageUrl: string; // Ensure nullability matches the .NET model if needed
  registrations?: Registration[];
  platinumTicketsOversubscribed?: number;
  goldTicketsOversubscribed?: number;
  silverTicketsOversubscribed?: number;
}
