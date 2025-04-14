import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "@/stores/store.ts";

// according to the docs it is recommended to create our own typed hooks instead of using the built-in (useSelector and useDispatch) =>
// https://redux-toolkit.js.org/tutorials/typescript#define-typed-hooks

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();
export const useAppSelector = useSelector.withTypes<RootState>();
