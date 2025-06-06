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
import { adminNav, studentNav, teacherNav } from "@/utils/constants/nav-main-content.ts";
import { type JSX } from "react";

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const { isAuthenticated, roles } = useAppSelector((state) => state.auth);
  let navElement: JSX.Element;

  if(isAuthenticated && roles.includes("Admin")) {
    navElement = <NavMain items={adminNav.navItems} isAuthenticated />
  }
  else if (isAuthenticated && roles.includes("Teacher") && !roles.includes("Admin")) {
    navElement = <NavMain items={teacherNav.navItems} isAuthenticated />
  }
  else if (isAuthenticated && roles.includes("Student") && !roles.includes("Admin")) {
    navElement = <NavMain items={studentNav.navItems} isAuthenticated />
  }
  else {
    navElement = <NavMain isAuthenticated={false} />
  }

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
        {navElement}
      </SidebarContent>
      <SidebarFooter>
        <NavUser />
      </SidebarFooter>
    </Sidebar>
  );
}
