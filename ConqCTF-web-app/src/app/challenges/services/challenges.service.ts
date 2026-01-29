import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  PaginatedList,
  ChallengeDto,
  ChallengeDetailsDto
} from '../models/challenge.models';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ChallengesService {

  private readonly apiUrl = `${environment.apiUrl}/api/challenges`;

  constructor(private http: HttpClient) { }

  getChallenges(pageNumber = 1, pageSize = 10): Observable<PaginatedList<ChallengeDto>> {
    return this.http.get<PaginatedList<ChallengeDto>>(
      `${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  getChallenge(id: number): Observable<ChallengeDetailsDto> {
    return this.http.get<ChallengeDetailsDto>(`${this.apiUrl}/${id}`);
  }

  submitFlag(id: number, flag: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/submit`, { flag });
  }

  downloadFile(id: number, fileName: string): Observable<Blob> {
    return this.http.get(
      `${this.apiUrl}/${id}/files/${fileName}`,
      { responseType: 'blob' }
    );
  }

  createChallenge(formData: FormData): Observable<number> {
    return this.http.post<number>(this.apiUrl, formData);
  }

  updateChallenge(id: number, formData: FormData): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
  }

  deleteChallenge(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/${id}`);
}
}