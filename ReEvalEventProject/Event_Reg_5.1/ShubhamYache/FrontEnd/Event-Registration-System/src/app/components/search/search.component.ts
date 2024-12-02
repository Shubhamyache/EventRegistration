import { Component, EventEmitter, Output } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [FormsModule, CommonModule],
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent {
  searchQuery: string = '';

  @Output() searchQueryChanged = new EventEmitter<string>();

  // Method to handle when the user types a search query
  onSearch() {
    this.searchQueryChanged.emit(this.searchQuery);
  }
}
