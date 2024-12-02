import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service'; // Adjust the path
import { PageEvent } from '@angular/material/paginator';
import { MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [MatPaginatorModule, CommonModule, FormsModule],
  selector: 'app-view-all-events',
  templateUrl: './all-events-list.component.html',
  styleUrls: ['./all-events-list.component.css'],
})
export class AllEventsListComponent implements OnInit {
  events: any[] = [];
  filteredEvents: any[] = [];
  pageSize = 5;
  pageIndex = 0;
  totalLength = 0;
  selectedCategory = '';

  categories: string[] = ['Technology', 'Business', 'Education'];

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.eventService.getAllEvents().subscribe((events) => {
      this.events = events;
      this.filteredEvents = events;
      this.totalLength = events.length;
      this.applyFilters();
    });
  }

  applyFilters(): void {
    this.filteredEvents = this.events.filter(
      (event) =>
        this.selectedCategory === '' || event.category === this.selectedCategory
    );
    this.totalLength = this.filteredEvents.length;
    this.paginate();
  }

  paginate(): void {
    const startIndex = this.pageIndex * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.filteredEvents = this.filteredEvents.slice(startIndex, endIndex);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.paginate();
  }

  onCategoryChange(category: string): void {
    this.selectedCategory = category;
    this.pageIndex = 0;
    this.applyFilters();
  }
}
