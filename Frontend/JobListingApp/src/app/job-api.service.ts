import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JobListing } from './job-listing-interface';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl = 'https://pam-stilling-feed.nav.no/api/v1/feed';

  constructor(private http: HttpClient) {}

  getData(): Observable<JobListing[]> {
    const headers = new HttpHeaders({
      Authorization: `Bearer <add-bearer token>`, // Make sure to replace with a valid token
      Accept: 'application/json',
    });

    return this.http
      .get<{ items: { _feed_entry: any }[] }>(this.apiUrl, { headers })
      .pipe(
        map((response) => {
          return response.items.map((item) => {
            const entry = item._feed_entry;
            return {
              id: entry.uuid,
              title: entry.title,
              businessName: entry.businessName,
              municipal: entry.municipal,
              status: entry.status,
              date_modified: entry.sistEndret,
            };
          });
        })
      );
  }
}
