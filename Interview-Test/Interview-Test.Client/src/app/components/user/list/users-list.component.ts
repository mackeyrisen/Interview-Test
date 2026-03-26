import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  standalone: true,
  selector: 'app-users-list',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './users-list.component.html',
})
export class UsersListComponent implements OnInit {
  users: any[] = [];
  searchText = '';

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getUsers().subscribe(data => this.users = data);
  }

  get filteredUsers(): any[] {
    const q = this.searchText.toLowerCase();
    if (!q) return this.users;
    return this.users.filter(u =>
      u.id?.toLowerCase().includes(q) ||
      u.userId?.toLowerCase().includes(q) ||
      u.username?.toLowerCase().includes(q) ||
      u.firstName?.toLowerCase().includes(q) ||
      u.lastName?.toLowerCase().includes(q)
    );
  }
}
