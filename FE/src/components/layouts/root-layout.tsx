import { AppSidebar } from "@/components/sidebar/app-sidebar.tsx";
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb.tsx";
import { Separator } from "@/components/ui/separator.tsx";
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "@/components/ui/sidebar.tsx";
import { Fragment, useEffect } from "react";
import useTheme from "@/features/theme/hooks/use-theme.ts";
import { Link, Outlet, useLocation } from "react-router";
import splitLocationPathname from "@/utils/functions/split-location-pathname.ts";
import { useLazyFetchUserProfileQuery } from "@/features/auth/api/auth-api.ts";
import { useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import LoadingSpinner from "@/components/spinners/loading-spinner.tsx";

export default function RootLayout() {
  const { theme } = useTheme();
  const location = useLocation();
  const {
    pathnameSplit,
    pathnameSplitWithoutLastElement,
    pathnameSplitLastElement,
  } = splitLocationPathname(location.pathname);
  const [fetchUserProfileTrigger] = useLazyFetchUserProfileQuery();
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);

  useEffect(() => {
    const root = window.document.documentElement;
    root.classList.remove("light", "dark");
    if (theme === "system") {
      const systemTheme = window.matchMedia("(prefers-color-scheme: dark)")
        .matches
        ? "dark"
        : "light";

      root.classList.add(systemTheme);
      return;
    }

    root.classList.add(theme);
  }, [theme]);

  useEffect(() => {
    fetchUserProfileTrigger();
  }, [fetchUserProfileTrigger]);

  return isAuthenticated !== null ? (
    <SidebarProvider>
      <AppSidebar />
      <SidebarInset>
        <header className="flex h-16 shrink-0 items-center gap-2">
          <div className="flex items-center gap-2 px-4">
            <SidebarTrigger className="-ml-1 cursor-pointer" />
            <Separator orientation="vertical" className="mr-2 h-4" />
            <Breadcrumb>
              <BreadcrumbList>
                {pathnameSplitWithoutLastElement.map((path, index) => (
                  <Fragment key={path} >
                    <BreadcrumbItem
                      className="hidden md:block capitalize"
                    >
                      <BreadcrumbLink asChild>
                        <Link to={pathnameSplit.slice(0, index + 1).join("/")}>
                          {path?.replaceAll("-", " ")}
                        </Link>
                      </BreadcrumbLink>
                    </BreadcrumbItem>
                    <BreadcrumbSeparator className="hidden md:block" />
                  </Fragment>
                ))}
                <BreadcrumbItem>
                  <BreadcrumbPage className="capitalize">
                    {pathnameSplitLastElement?.replaceAll("-", " ") ?? "Home"}
                  </BreadcrumbPage>
                </BreadcrumbItem>
              </BreadcrumbList>
            </Breadcrumb>
          </div>
        </header>
        <section className="bg-background relative flex w-full flex-1 flex-col">
          <Outlet />
        </section>
      </SidebarInset>
    </SidebarProvider>
  ) : (
    <LoadingSpinner text="Loading user profile..." />
  );
}
