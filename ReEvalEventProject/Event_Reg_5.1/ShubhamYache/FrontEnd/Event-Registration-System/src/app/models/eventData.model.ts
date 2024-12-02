export interface EventData {
  eventName: string;
  category: string;
  eventStartDateTime: Date;
  eventEndDateTime: Date;
  registrationCloseDate: Date;
  minimumAge: number;
  platinumTicketsNumber: number;
  platinumTicketsPrice: number;
  description: string;
  goldTicketsNumber: number;
  goldTicketsPrice: number;
  silverTicketsNumber: number;
  silverTicketsPrice: number;
  eventStatus: string;
  hashtag: string;
  guests: string;
  imageUrl: string;
  organizerEmail: string;
  venueId: number;
}
