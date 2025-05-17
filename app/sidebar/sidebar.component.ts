import { Component, Output, EventEmitter } from '@angular/core'; // Added Output and EventEmitter
import { CommonModule } from '@angular/common';

// Define the possible navigation items as a type for better type safety
export type SidebarNavItem = 'dashboard' | 'favorites' | 'about';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule], // CommonModule for *ngIf, *ngFor, etc. if needed in template
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  // Property to keep track of the currently active item
  activeItem: SidebarNavItem = 'dashboard'; // Default to 'dashboard'

  // Optional: If you want to notify the parent component about navigation
  @Output() navigate = new EventEmitter<SidebarNavItem>();
  @Output() signOut = new EventEmitter<void>();

  constructor() { }

  // Method to set the active item
  setActive(item: SidebarNavItem): void {
    if (this.activeItem === item) {
      return; // Don't do anything if already active
    }
    this.activeItem = item;
    console.log('Sidebar item clicked, new active item:', this.activeItem);

    // Optionally emit an event to the parent component
    this.navigate.emit(this.activeItem);

    // If using Angular Router, you would navigate here:
    // this.router.navigate(['/' + item]); // Example
  }

  // Method to handle sign out
  handleSignOut(): void {
    console.log('Sign out button clicked');
    // Implement actual sign out logic here (e.g., call an auth service)
    this.signOut.emit(); // Notify parent about sign out attempt
  }
}