import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest } from '../models/employee.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly API_URL = 'https://localhost:7000/api/employees'; // Update with your backend URL

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.API_URL, {
      headers: this.getAuthHeaders()
    });
  }

  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.API_URL}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  createEmployee(employee: CreateEmployeeRequest): Observable<Employee> {
    return this.http.post<Employee>(this.API_URL, employee, {
      headers: this.getAuthHeaders()
    });
  }

  updateEmployee(employee: UpdateEmployeeRequest): Observable<Employee> {
    return this.http.put<Employee>(`${this.API_URL}/${employee.id}`, employee, {
      headers: this.getAuthHeaders()
    });
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }
}