export type AuthState = {
  isAuthenticated: boolean | null;
  email: string | null;
  roles: string[];
};

export type LoginData = {
  email: string;
  roles: string[];
};

export type UserLogin = {
  email: string;
  password: string;
};

export type UserRegister = {
  email: string;
  password: string;
  //TODO: add more fields if needed in the future
};

export type TeacherRegister = {
  email: string;
  password: string;
  secret: string;
};
