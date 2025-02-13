import { Component } from '@angular/core';
import { ApiService } from './job-api.service';
import { JobTableComponent } from './job-table/job-table.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [JobTableComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'JobListingApp';
  constructor(private apiService: ApiService) {}
}
