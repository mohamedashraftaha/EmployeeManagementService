import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { Employee } from '../../models/employee.model';
import { EmployeeService } from '../../services/employee.service';
import { AuthService } from '../../services/auth.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
  template: `
    <mat-toolbar color="primary">
      <span>Employee Management System</span>
      <span class="spacer"></span>
      <span class="user-info">Welcome, {{currentUser?.firstName}} {{currentUser?.lastName}}</span>
      <button mat-icon-button (click)="logout()">
        <mat-icon>logout</mat-icon>
      </button>
    </mat-toolbar>

    <div class="container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>Employees</mat-card-title>
          <div class="spacer"></div>
          <button mat-raised-button color="primary" (click)="addEmployee()">
            <mat-icon>add</mat-icon>
            Add Employee
          </button>
        </mat-card-header>

        <mat-card-content>
          <div style="margin-bottom: 16px;">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Search employees</mat-label>
              <input matInput [(ngModel)]="searchTerm" placeholder="Type to search by name, email, or position">
              <button *ngIf="searchTerm" matSuffix mat-icon-button aria-label="Clear" (click)="searchTerm = ''">
                <mat-icon>close</mat-icon>
              </button>
            </mat-form-field>
          </div>
          <div class="table-container">
            <table mat-table [dataSource]="filteredEmployees" class="employee-table">
              <ng-container matColumnDef="firstName">
                <th mat-header-cell *matHeaderCellDef>First Name</th>
                <td mat-cell *matCellDef="let employee">{{employee.firstName}}</td>
              </ng-container>

              <ng-container matColumnDef="lastName">
                <th mat-header-cell *matHeaderCellDef>Last Name</th>
                <td mat-cell *matCellDef="let employee">{{employee.lastName}}</td>
              </ng-container>

              <ng-container matColumnDef="email">
                <th mat-header-cell *matHeaderCellDef>Email</th>
                <td mat-cell *matCellDef="let employee">{{employee.email}}</td>
              </ng-container>

              <ng-container matColumnDef="position">
                <th mat-header-cell *matHeaderCellDef>Position</th>
                <td mat-cell *matCellDef="let employee">{{employee.position}}</td>
              </ng-container>

              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef>Actions</th>
                <td mat-cell *matCellDef="let employee">
                  <button mat-icon-button color="primary" (click)="editEmployee(employee.id)" matTooltip="Edit">
                    <mat-icon>edit</mat-icon>
                  </button>
                  <button mat-icon-button color="warn" (click)="deleteEmployee(employee.id)" matTooltip="Delete">
                    <mat-icon>delete</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
            </table>
          </div>

          <div *ngIf="employees.length === 0" class="no-data">
            <mat-icon>people_outline</mat-icon>
            <p>No employees found</p>
            <button mat-raised-button color="primary" (click)="addEmployee()">
              Add First Employee
            </button>
          </div>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
      max-width: 1200px;
      margin: 0 auto;
    }

    .spacer {
      flex: 1 1 auto;
    }

    .user-info {
      margin-right: 16px;
      font-size: 14px;
    }

    mat-card-header {
      display: flex;
      align-items: center;
      margin-bottom: 20px;
    }

    .table-container {
      overflow-x: auto;
    }

    .employee-table {
      width: 100%;
      min-width: 800px;
    }

    .status-badge {
      padding: 4px 12px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
    }

    .status-badge.active {
      background-color: #e8f5e8;
      color: #2e7d32;
    }

    .status-badge.inactive {
      background-color: #ffebee;
      color: #c62828;
    }

    .no-data {
      text-align: center;
      padding: 40px;
      color: #666;
    }

    .no-data mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      margin-bottom: 16px;
    }

    .no-data p {
      font-size: 18px;
      margin-bottom: 20px;
    }

    mat-toolbar {
      position: sticky;
      top: 0;
      z-index: 100;
    }
  `]
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'position', 'actions'];
  searchTerm: string = '';
  currentUser = this.authService.getCurrentUser();

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.employeeService.getAllEmployees().subscribe({
      next: (employees) => {
        this.employees = employees;
      },
      error: (error) => {
        this.snackBar.open('Failed to load employees', 'Close', { duration: 5000 });
      }
    });
  }

  addEmployee(): void {
    this.router.navigate(['/employees/add']);
  }

  editEmployee(id: number): void {
    this.router.navigate(['/employees/edit', id]);
  }

  deleteEmployee(id: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Employee',
        message: 'Are you sure you want to delete this employee? This action cannot be undone.',
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.employeeService.deleteEmployee(id).subscribe({
          next: () => {
            this.snackBar.open('Employee deleted successfully', 'Close', { duration: 3000 });
            this.loadEmployees();
          },
          error: (error) => {
            this.snackBar.open('Failed to delete employee', 'Close', { duration: 5000 });
          }
        });
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  get filteredEmployees(): Employee[] {
    if (!this.searchTerm) return this.employees;
    const term = this.searchTerm.toLowerCase();
    return this.employees.filter(emp =>
      emp.firstName.toLowerCase().includes(term) ||
      emp.lastName.toLowerCase().includes(term) ||
      emp.email.toLowerCase().includes(term) ||
      emp.position.toLowerCase().includes(term)
    );
  }
}