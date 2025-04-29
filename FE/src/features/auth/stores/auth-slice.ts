import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { AuthState, LoginDataResponseDto } from "@/features/auth/types/auth-types.ts";

const initialState: AuthState = {
  isAuthenticated: null,
  email: null,
  roles: [],
};

export const authSlice = createSlice({
  name: "auth",
  initialState: initialState,
  reducers: {
    initialLoad: (state) => {
      state.isAuthenticated = false;
    },
    login: (state, action: PayloadAction<LoginDataResponseDto>) => {
      state.isAuthenticated = true;
      state.email = action.payload.email;
      state.roles = action.payload.roles;
    },
    logout: (state) => {
      state.isAuthenticated = false;
      state.email = null;
      state.roles = [];
    },
  },
});

export const { initialLoad, login, logout } = authSlice.actions;

export default authSlice.reducer;
