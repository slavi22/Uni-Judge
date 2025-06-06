import { baseApi } from "@/stores/base-api.ts";
import type {
  LoginDataResponseDto,
  TeacherRegisterDto,
  UserLoginDto,
  UserRegisterDto,
} from "@/features/auth/types/auth-types.ts";
import {
  initialLoad,
  login,
  logout,
} from "@/features/auth/stores/auth-slice.ts";
import { BE_URL } from "@/utils/constants/consts.ts";
import { toast } from "sonner";

const authApi = baseApi.injectEndpoints({
  endpoints: (build) => ({
    // for the generic, the left one is the return type, the right one is the argument (parameters) type
    login: build.mutation<void, UserLoginDto>({
      // fixed the queryFn issue related to the return type => https://redux-toolkit.js.org/rtk-query/usage/customizing-queries#fetchbasequery-defaults
      query: (loginData) => ({
        url: "auth/login",
        method: "POST",
        body: loginData, // we dont need to JSON.stringify here since RTK Query does it for us
      }),
      async onQueryStarted(_args, { dispatch, queryFulfilled }) {
        try {
          await queryFulfilled;
          const userInfo = await fetch(BE_URL + "auth/me", {
            credentials: "include",
          });
          const userData = (await userInfo.json()) as LoginDataResponseDto;
          dispatch(login(userData));
        } catch {
          toast.error("Error logging in.");
        }
      },
      // queryFn equivalent
      /*queryFn: async (loginData, { dispatch }) => {
                                      const response = await fetch(BE_URL + "auth/login", {
                                        method: "POST",
                                        headers: {
                                          "Content-Type": "application/json",
                                        },
                                        body: JSON.stringify(loginData),
                                        credentials: "include",
                                      });
                                      if (!response.ok) {
                                        return {
                                          error: {
                                            status: response.status,
                                            data: response.statusText,
                                          },
                                        };
                                      }
                                      const userInfo = await fetch(BE_URL + "auth/me", {
                                        credentials: "include",
                                      });
                                      const userData = (await userInfo.json()) as LoginData;
                                      dispatch(login(userData));
                                      return { data: undefined };
                                    },*/
    }),
    fetchUserProfile: build.query<LoginDataResponseDto, void>({
      query: () => "auth/me",
      async onQueryStarted(_args, { dispatch, queryFulfilled }) {
        try {
          const response = await queryFulfilled;
          dispatch(login(response.data));
        } catch {
          dispatch(initialLoad());
        }
      },
      // queryFn equivalent
      /*queryFn: async (_args, { dispatch }) => {
                                const userProfileResponse = await fetch(BE_URL + "auth/me", {
                                  credentials: "include",
                                });
                                if (!userProfileResponse.ok) {
                                  dispatch(initialLoad());
                                  return {
                                    error: {
                                      status: userProfileResponse.status,
                                      data: userProfileResponse.statusText,
                                    },
                                  };
                                }
                                const userData = (await userProfileResponse.json()) as LoginData;
                                dispatch(login(userData));
                                return { data: userData };
                              },*/
    }),
    logout: build.mutation<void, void>({
      query: () => ({ url: "auth/logout", method: "POST" }),
      async onQueryStarted(_args, { dispatch, queryFulfilled }) {
        await queryFulfilled;
        dispatch(logout());
      },
    }),
    register: build.mutation<string, UserRegisterDto>({
      query: (registerData) => ({
        url: "auth/register",
        method: "POST",
        body: registerData,
      }),
      async onQueryStarted(_args, { queryFulfilled }) {
        try {
          await queryFulfilled;
          toast.success("Registration successful. You can now log in.", {
            closeButton: true,
          });
        } catch {
          toast.error("Error registering.");
        }
      },
    }),
    registerTeacher: build.mutation<string, TeacherRegisterDto>({
      query: (registerTeacherData) => ({
        url: "auth/register-teacher",
        method: "POST",
        body: registerTeacherData,
      }),
      async onQueryStarted(_args, { queryFulfilled }) {
        try {
          await queryFulfilled;
          toast.success("Teacher registration successful. You can now log in.");
        } catch {
          toast.error("Error registering.");
        }
      },
    }),
  }),
});

export const {
  useLoginMutation,
  useLazyFetchUserProfileQuery,
  useLogoutMutation,
  useRegisterMutation,
  useRegisterTeacherMutation,
} = authApi;
