export interface Employee {
  id?: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  department: string;
  position: string;
  salary?: number;
  hireDate: Date;
  isActive: boolean;
}

export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  department: string;
  position: string;
  salary?: number;
  hireDate: Date;
}

export interface UpdateEmployeeRequest extends CreateEmployeeRequest {
  id: number;
  isActive: boolean;
}