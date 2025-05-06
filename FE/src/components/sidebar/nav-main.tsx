"use client";

import { ChevronRight, LogInIcon, type LucideIcon } from "lucide-react";

import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible.tsx";
import {
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubButton,
  SidebarMenuSubItem,
} from "@/components/ui/sidebar.tsx";
import { Link } from "react-router";

type InnerItems = {
  title: string;
  url: string;
};

type OuterItems = {
  title: string;
  icon: LucideIcon;
  isActive?: boolean;
  items?: InnerItems[];
};

type NavProps = {
  items?: OuterItems[];
  isAuthenticated: boolean;
};

export function NavMain({ items, isAuthenticated }: NavProps) {
  const allCourses =
    items && items.find((item) => item.title === "All Courses");
  const teacherAdminItems =
    items &&
    items.filter((item) => item.title === "Teacher" || item.title === "Admin");

  return (
    <>
      <SidebarGroup>
        <SidebarGroupLabel>Student</SidebarGroupLabel>
        <SidebarMenu>
          {items &&
            items
              .filter(
                (item) =>
                  item.title !== "All Courses" &&
                  item.title !== "Teacher" &&
                  item.title !== "Admin",
              )
              .map((item) => (
                <Collapsible
                  key={item.title}
                  asChild
                  defaultOpen={item.isActive}
                  className="group/collapsible"
                >
                  <SidebarMenuItem>
                    {/*TODO: REMOVE ONCE DONE TESTING*/}
                    <div className="mb-10">
                      <Link to="/code-editor-test">Code editor test page</Link>
                    </div>
                    <CollapsibleTrigger className="cursor-pointer" asChild>
                      <SidebarMenuButton tooltip={item.title}>
                        {item.icon && <item.icon />}
                        <span>{item.title}</span>
                        <ChevronRight className="ml-auto transition-transform duration-200 group-data-[state=open]/collapsible:rotate-90" />
                      </SidebarMenuButton>
                    </CollapsibleTrigger>
                    <CollapsibleContent>
                      <SidebarMenuSub>
                        {item.items?.map((subItem) => (
                          <SidebarMenuSubItem key={subItem.title}>
                            <SidebarMenuSubButton asChild>
                              <Link to={subItem.url}>
                                <span>{subItem.title}</span>
                              </Link>
                            </SidebarMenuSubButton>
                          </SidebarMenuSubItem>
                        ))}
                      </SidebarMenuSub>
                    </CollapsibleContent>
                  </SidebarMenuItem>
                </Collapsible>
              ))}
          <SidebarMenuItem>
            {isAuthenticated ? (
              <>
                <SidebarMenuButton asChild>
                  <Link to="/courses/all-courses">
                    {" "}
                    {/* TODO: Replace with the real link */}
                    {allCourses && allCourses.icon && <allCourses.icon />}
                    <span>{allCourses && allCourses.title}</span>
                  </Link>
                </SidebarMenuButton>
              </>
            ) : (
              <SidebarMenuButton asChild>
                <Link to="/login">
                  <LogInIcon />
                  Login to view all options
                </Link>
              </SidebarMenuButton>
            )}
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarGroup>
      {teacherAdminItems &&
        teacherAdminItems.map((item) => (
          <SidebarGroup key={item.title}>
            <SidebarGroupLabel>{item.title}</SidebarGroupLabel>
            <SidebarMenu>
              <Collapsible
                key={item.title}
                asChild
                defaultOpen={item.isActive}
                className="group/collapsible"
              >
                <SidebarMenuItem>
                  <CollapsibleTrigger className="cursor-pointer" asChild>
                    <SidebarMenuButton tooltip={item.title}>
                      {item.icon && <item.icon />}
                      <span>{item.title}</span>
                      <ChevronRight className="ml-auto transition-transform duration-200 group-data-[state=open]/collapsible:rotate-90" />
                    </SidebarMenuButton>
                  </CollapsibleTrigger>
                  <CollapsibleContent>
                    <SidebarMenuSub>
                      {item.items?.map((subItem) => (
                        <SidebarMenuSubItem key={subItem.title}>
                          <SidebarMenuSubButton asChild>
                            <Link to={subItem.url}>
                              <span>{subItem.title}</span>
                            </Link>
                          </SidebarMenuSubButton>
                        </SidebarMenuSubItem>
                      ))}
                    </SidebarMenuSub>
                  </CollapsibleContent>
                </SidebarMenuItem>
              </Collapsible>
            </SidebarMenu>
          </SidebarGroup>
        ))}
    </>
  );
}
