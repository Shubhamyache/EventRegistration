import { Component, OnInit } from '@angular/core';
import { UserAuthService } from '../../services/auth.service'; // Adjust the path
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  standalone: true,
  imports: [MatPaginatorModule, CommonModule, FormsModule],
  selector: 'app-view-all-users',
  templateUrl: './all-users-list.component.html',
  styleUrls: ['./all-users-list.component.css'],
})
export class AllUsersListComponent implements OnInit {
  users: any[] = [];
  filteredUsers: any[] = [];
  pageSize = 5;
  pageIndex = 0;
  totalLength = 0;
  searchQuery = '';

  constructor(private userService: UserAuthService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAllUsers().subscribe((users) => {
      this.users = users;
      this.filteredUsers = users;
      this.totalLength = users.length;
      this.applyFilters();
    });
  }

  applyFilters(): void {
    this.filteredUsers = this.users.filter(
      (user) =>
        user.firstName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.lastName.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
    this.totalLength = this.filteredUsers.length;
    this.paginate();
  }

  paginate(): void {
    const startIndex = this.pageIndex * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.filteredUsers = this.filteredUsers.slice(startIndex, endIndex);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.paginate();
  }

  onSearch(query: string): void {
    this.searchQuery = query;
    this.pageIndex = 0;
    this.applyFilters();
  }
}
