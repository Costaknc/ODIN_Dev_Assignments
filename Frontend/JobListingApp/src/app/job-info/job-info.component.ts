import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-job-info',
  templateUrl: './job-info.component.html',
  styleUrl: './job-info.component.css',
  imports: [CommonModule],
})
export class JobInfoComponent {
  @Input() details: {
    id: string;
    title: string;
    businessName: string;
    municipal: string;
    status: string;
    date_modified: string;
  } | null = null;
}
