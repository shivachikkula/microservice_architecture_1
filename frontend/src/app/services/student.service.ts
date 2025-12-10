import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { StudentDetails } from '../models/student-details.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private apiUrl = `${environment.apiConfig.uri}/student`;

  constructor(private http: HttpClient) {}

  saveStudent(student: StudentDetails): Observable<any> {
    return this.http.post(`${this.apiUrl}`, student);
  }

  getStudent(id: string): Observable<StudentDetails> {
    return this.http.get<StudentDetails>(`${this.apiUrl}/${id}`);
  }

  getAllStudents(): Observable<StudentDetails[]> {
    return this.http.get<StudentDetails[]>(`${this.apiUrl}`);
  }
}
