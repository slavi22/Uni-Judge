import { useAppDispatch, useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import { changeTheme } from "@/features/theme/stores/theme-slice.ts";
import { Theme } from "@/features/theme/types/theme-types.ts";

export default function useTheme() {
  const dispatch = useAppDispatch();
  const theme = useAppSelector((state) => state.theme.theme);
  const setTheme = (theme: Theme) => dispatch(changeTheme(theme));
  return { theme, setTheme };
}
