import { configureStore } from "@reduxjs/toolkit";
import themeSliceReducer, {
  themeSlice,
} from "../features/theme/stores/theme-slice.ts";
import authSliceReducer, {
  authSlice,
} from "@/features/auth/stores/auth-slice.ts";
import { baseApi } from "@/stores/base-api.ts";
import {setupListeners} from "@reduxjs/toolkit/query";

export const store = configureStore({
  reducer: {
    // apis
    [baseApi.reducerPath]: baseApi.reducer,
    // slices
    [authSlice.name]: authSliceReducer,
    [themeSlice.name]: themeSliceReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(baseApi.middleware),
});

setupListeners(store.dispatch)

// according to the docs => https://redux-toolkit.js.org/tutorials/quick-start#create-a-redux-store
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
