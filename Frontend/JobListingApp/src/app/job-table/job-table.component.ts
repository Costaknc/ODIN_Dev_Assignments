import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobListing } from '../job-listing-interface';
import { ApiService } from '../job-api.service';
import { JobInfoComponent } from '../job-info/job-info.component';

@Component({
  selector: 'app-job-table',
  imports: [CommonModule, JobInfoComponent],
  templateUrl: './job-table.component.html',
  styleUrls: ['./job-table.component.css'],
})
export class JobTableComponent implements OnInit {
  jobListings: JobListing[] = [];
  paginatedData: JobListing[] = [];
  expandedRows: boolean[] = [];
  selectedItem: JobListing | null = null;

  itemsPerPage = 13;
  currentPage = 1;
  totalPages = 0;
  maxPagesToShow = 5;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.apiService.getData().subscribe((listings) => {
      this.jobListings = listings;
      this.totalPages = Math.ceil(this.jobListings.length / this.itemsPerPage);
      this.updatePaginatedData();
    });
  }

  updatePaginatedData() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    this.paginatedData = this.jobListings.slice(
      start,
      start + this.itemsPerPage
    );
    this.expandedRows = Array(this.paginatedData.length).fill(false);
  }

  changePage(page: number) {
    if (page > 0 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePaginatedData();
    }
  }

  toggleInfo(index: number) {
    this.expandedRows[index] = !this.expandedRows[index];
    this.selectedItem = this.expandedRows[index]
      ? this.paginatedData[index]
      : null;
  }

  get pages() {
    const halfMaxPages = Math.floor(this.maxPagesToShow / 2);
    let startPage = Math.max(1, this.currentPage - halfMaxPages);
    let endPage = Math.min(this.totalPages, this.currentPage + halfMaxPages);

    if (this.currentPage - halfMaxPages <= 0) {
      endPage = Math.min(this.totalPages, this.maxPagesToShow);
    }
    if (this.currentPage + halfMaxPages > this.totalPages) {
      startPage = Math.max(1, this.totalPages - this.maxPagesToShow + 1);
    }
    return Array.from(
      { length: endPage - startPage + 1 },
      (_, i) => startPage + i
    );
  }
}
