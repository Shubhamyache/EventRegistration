export interface Venue {
  venueId: number;
  venueName: string;
  addressLine1: string;
  city: string;
  maxCapacity: number;
  hasSeats: number;
  events?: Event[] | null;
}
