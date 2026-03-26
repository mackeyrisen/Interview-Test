import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const GATEWAY = 'http://localhost:5051/gateway/api/user';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private http: HttpClient) {}

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${GATEWAY}/GetUsers`, {
      headers: { 'x-api-key': 'interview-test' }
    });
  }

  getUserById(id: string): Observable<any> {
    return this.http.get<any>(`${GATEWAY}/GetUserById/${id}`, {
      headers: { 'x-api-key': 'interview-test' }
    });
  }
}
