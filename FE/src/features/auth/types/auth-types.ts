export type AuthState = {
  isAuthenticated: boolean | null;
  email: string | null;
  roles: string[];
};

export type LoginData = {
  email: string;
  roles: string[];
}

