import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common'; 

@Component({
  selector: 'app-stock-details',
  standalone: true,
  templateUrl: './stock-details.component.html',
  styleUrls: ['./stock-details.component.css'],
  imports: [CommonModule]
})
export class StockDetailsComponent {
  @Input() stockData: any[] = []; 
}
