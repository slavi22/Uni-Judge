import useTheme from "@/features/theme/hooks/use-theme.ts";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu.tsx";
import { Moon, Sun } from "lucide-react";

export default function ThemeToggle() {
  const { theme, setTheme } = useTheme();
  const systemTheme = window.matchMedia("(prefers-color-scheme: dark)").matches
    ? "dark"
    : "light";
  const activeTheme = theme === "system" ? systemTheme : theme;
  const isLight = activeTheme === "light";

  function handleClick() {
    if (theme === "system") {
      setTheme(systemTheme === "light" ? "dark" : "light");
      return;
    }
    setTheme(theme === "light" ? "dark" : "light");
  }

  return (
    // onSelect meaning => https://github.com/shadcn-ui/ui/issues/2677#issuecomment-2668027089
    <DropdownMenuItem onClick={handleClick} onSelect={(e) => e.preventDefault()}>
      {isLight ? <Moon/> : <Sun/>}
      Toggle {isLight ? "dark" : "light"} mode
    </DropdownMenuItem>
  );
}
