import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Employee } from '../../models/employee.model';
import { EmployeeService } from '../../services/employee.service';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSlideToggleModule,
    MatSnackBarModule
  ],
  template: `
    <div class="container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <button mat-icon-button (click)="goBack()" class="back-button">
              <mat-icon>arrow_back</mat-icon>
            </button>
            {{isEditMode ? 'Edit Employee' : 'Add New Employee'}}
          </mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="employeeForm" (ngSubmit)="onSubmit()">
            <div class="form-row">
              <mat-form-field appearance="outline" class="half-width">
                <mat-label>First Name</mat-label>
                <input matInput formControlName="firstName" required>
                <mat-error *ngIf="employeeForm.get('firstName')?.hasError('required')">
                  First name is required
                </mat-error>
              </mat-form-field>

              <mat-form-field appearance="outline" class="half-width">
                <mat-label>Last Name</mat-label>
                <input matInput formControlName="lastName" required>
                <mat-error *ngIf="employeeForm.get('lastName')?.hasError('required')">
                  Last name is required
                </mat-error>
              </mat-form-field>
            </div>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Email</mat-label>
              <input matInput type="email" formControlName="email" required>
              <mat-icon matSuffix>email</mat-icon>
              <mat-error *ngIf="employeeForm.get('email')?.hasError('required')">
                Email is required
              </mat-error>
              <mat-error *ngIf="employeeForm.get('email')?.hasError('email')">
                Please enter a valid email
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Position</mat-label>
              <input matInput formControlName="position" required>
              <mat-error *ngIf="employeeForm.get('position')?.hasError('required')">
                Position is required
              </mat-error>
            </mat-form-field>

            <div class="form-actions">
              <button mat-button type="button" (click)="goBack()" class="cancel-button">
                Cancel
              </button>
              <button mat-raised-button color="primary" type="submit" 
                      [disabled]="employeeForm.invalid || isLoading">
                {{isLoading ? 'Saving...' : (isEditMode ? 'Update Employee' : 'Add Employee')}}
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
      max-width: 800px;
      margin: 0 auto;
    }

    .back-button {
      margin-right: 8px;
    }

    .form-row {
      display: flex;
      gap: 16px;
      align-items: flex-start;
    }

    .half-width {
      flex: 1;
      margin-bottom: 16px;
    }

    .full-width {
      width: 100%;
      margin-bottom: 16px;
    }

    .status-toggle {
      margin-bottom: 24px;
    }

    .form-actions {
      display: flex;
      gap: 16px;
      justify-content: flex-end;
      margin-top: 24px;
    }

    .cancel-button {
      color: #666;
    }

    mat-card-header {
      margin-bottom: 24px;
    }

    mat-card-title {
      display: flex;
      align-items: center;
      font-size: 24px;
    }

    @media (max-width: 600px) {
      .form-row {
        flex-direction: column;
        gap: 0;
      }

      .half-width {
        width: 100%;
      }

      .form-actions {
        flex-direction: column;
      }
    }
  `]
})
export class EmployeeFormComponent implements OnInit {
  employeeForm: FormGroup;
  isEditMode = false;
  isLoading = false;
  employeeId?: number;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    this.employeeForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      position: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.employeeId = +id;
      this.loadEmployee();
    }
  }

  loadEmployee(): void {
    if (this.employeeId) {
      this.employeeService.getEmployeeById(this.employeeId).subscribe({
        next: (employee) => {
          this.employeeForm.patchValue({
            ...employee,
          });
        },
        error: (error) => {
          this.snackBar.open('Failed to load employee', 'Close', { duration: 5000 });
          this.goBack();
        }
      });
    }
  }

  onSubmit(): void {
    if (this.employeeForm.valid) {
      this.isLoading = true;
      const formData = { ...this.employeeForm.value };
      
      if (this.isEditMode && this.employeeId) {
        formData.id = this.employeeId;
        this.employeeService.updateEmployee(formData).subscribe({
          next: () => {
            this.snackBar.open('Employee updated successfully', 'Close', { duration: 3000 });
            this.goBack();
          },
          error: (error) => {
            this.isLoading = false;
            this.snackBar.open('Failed to update employee', 'Close', { duration: 5000 });
          }
        });
      } else {
        this.employeeService.createEmployee(formData).subscribe({
          next: () => {
            this.snackBar.open('Employee created successfully', 'Close', { duration: 3000 });
            this.goBack();
          },
          error: (error) => {
            this.isLoading = false;
            this.snackBar.open('Failed to create employee', 'Close', { duration: 5000 });
          }
        });
      }
    }
  }

  goBack(): void {
    this.router.navigate(['/employees']);
  }
}