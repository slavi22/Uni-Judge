import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { Theme, ThemeState } from "@/features/theme/types/theme-types.ts";

const initialState: ThemeState = {
  theme: (localStorage.getItem("theme") as Theme) || "system",
};

export const themeSlice = createSlice({
  name: "theme",
  initialState: initialState,
  reducers: {
    changeTheme: (state, action: PayloadAction<Theme>) => {
      state.theme = action.payload;
      localStorage.setItem("theme", action.payload);
    },
  },
});

export const { changeTheme } = themeSlice.actions;

export default themeSlice.reducer;