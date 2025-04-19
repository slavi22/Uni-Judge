import {
  type BaseQueryFn,
  type FetchArgs,
  type FetchBaseQueryError,
  createApi,
  fetchBaseQuery,
} from "@reduxjs/toolkit/query/react";
import { BE_URL } from "@/utils/constants/consts.ts";
import { initialLoad, login } from "@/features/auth/stores/auth-slice.ts";
import { type LoginData } from "@/features/auth/types/auth-types.ts";

const baseQuery = fetchBaseQuery({ baseUrl: BE_URL, credentials: "include" });

// the baseQueryWithReauth only works when using query (IT WONT WORK IF WE USE "queryFn")
const baseQueryWithReauth: BaseQueryFn<
  string | FetchArgs,
  unknown,
  FetchBaseQueryError
> = async (args, api, extraOptions) => {
  let result = await baseQuery(args, api, extraOptions);
  if (result.error && result.error.status === 401) {
    const refreshResult = await baseQuery(
      {
        url: "auth/refresh-token",
        method: "POST",
      },
      api,
      extraOptions,
    );
    if (refreshResult.meta?.response?.status === 200) {
      result = await baseQuery("auth/me", api, extraOptions);
      api.dispatch(login(result.data as LoginData));
      //result = await baseQuery(args, api, extraOptions);
    } else {
      // set the login state to the initial since we couldn't refresh the token successfully
      // after we call the initialLoad if we are on a protected page we will be redirected to the login page automatically, thus we dont need to implement manual redirection
      api.dispatch(initialLoad());
    }
  }
  //console.log(result);
  return result;
};

export const baseApi = createApi({
  reducerPath: "baseApi",
  //keepUnusedDataFor: 5, //used to set the cache time for the data
  baseQuery: baseQueryWithReauth,
  endpoints: () => ({}),
});
