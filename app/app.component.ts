import { Component } from '@angular/core';
import { StockListComponent } from './stock-list/stock-list.component';
import { StockDetailsComponent } from './stock-details/stock-details.component';
import { SidebarComponent } from './sidebar/sidebar.component';

@Component({
  selector: 'app-root',
  standalone: true, // ✅ Convert AppComponent to standalone
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [StockListComponent, StockDetailsComponent,SidebarComponent] // ✅ Import standalone components directly
})
export class AppComponent {
  stockData: any[] = [];
}
