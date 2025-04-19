type FetchBaseQueryErrorCustom = {
  data: {
    detail: string;
    status: number;
    title: string;
  };
  status: number;
};

// the return type is - type predicate (maybe google for future reference)
// rtk query example => https://redux-toolkit.js.org/rtk-query/usage-with-typescript#inline-error-handling-example
export function isFetchBaseQueryError(
  error: unknown,
): error is FetchBaseQueryErrorCustom {
  return (
    typeof error === "object" &&
    error != null &&
    "status" in error &&
    "data" in error &&
    typeof error.data === "object" &&
    error.data !== null &&
    "detail" in error.data
  );
}
