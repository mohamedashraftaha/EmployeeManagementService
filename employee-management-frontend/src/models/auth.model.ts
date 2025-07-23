export interface LoginRequest {
  username: string;
  passwordhash: string;
}

export interface RegisterRequest {
  username: string;
  passwordhash: string;
}

export interface AuthResponse {
  token: string;
  user: {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
  };
}

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
}