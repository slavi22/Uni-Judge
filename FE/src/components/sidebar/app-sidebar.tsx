import { University } from "lucide-react";
import { NavMain } from "@/components/sidebar/nav-main.tsx";
import { NavUser } from "@/components/sidebar/nav-user.tsx";
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar.tsx";
import { Link } from "react-router";
import { useAppSelector } from "@/hooks/redux/redux-hooks.ts";
import { nav } from "@/utils/constants/nav-main-content.ts";

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);

  return (
    <Sidebar variant="inset" {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton size="lg" asChild>
              <Link to="/">
                <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
                  <University className="size-5" />
                </div>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-medium">Uni-Judge</span>
                  <span className="truncate text-xs">System</span>
                </div>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        {isAuthenticated ? (
          <NavMain items={nav.main} isAuthenticated />
        ) : (
          <NavMain isAuthenticated={false} />
        )}
      </SidebarContent>
      <SidebarFooter>
        <NavUser />
      </SidebarFooter>
    </Sidebar>
  );
}
