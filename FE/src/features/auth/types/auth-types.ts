export type AuthState = {
  isAuthenticated: boolean | null;
  email: string | null;
  roles: string[];
};

export type LoginDataResponseDto = {
  email: string;
  roles: string[];
};

export type UserLoginDto = {
  email: string;
  password: string;
};

export type UserRegisterDto = {
  email: string;
  password: string;
  //TODO: add more fields if needed in the future
};

export type TeacherRegisterDto = {
  email: string;
  password: string;
  secret: string;
};
