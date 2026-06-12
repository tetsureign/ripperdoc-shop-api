import * as React from "react";
import { useLocation, Link } from "react-router";

import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarRail,
} from "@/components/ui/sidebar";

import { APP_NAME, APP_ROUTES, UI_LABELS } from "@/lib/routes";
import { NavUser } from "./nav-user";

type SidebarProps = {
  sendTitle: (title: string) => void;
  userEmail: string;
  logout: () => void;
} & React.ComponentProps<typeof Sidebar>;

const data = {
  header: [
    {
      title: APP_NAME,
      url: APP_ROUTES.dashboard.url,
    },
  ],
  navMain: [
    {
      title: `${UI_LABELS.products} Management`,
      // url: APP_ROUTES.dashboard,
      items: [
        {
          title: APP_ROUTES.categories.name,
          url: APP_ROUTES.categories.url,
        },
        {
          title: APP_ROUTES.brands.name,
          url: APP_ROUTES.brands.url,
        },
        {
          title: APP_ROUTES.products.name,
          url: APP_ROUTES.products.url,
        },
      ],
    },
    {
      title: `${APP_ROUTES.customers.name} Management`,
      url: APP_ROUTES.customers.url,
      items: [
        {
          title: `${APP_ROUTES.customers.name} List`,
          url: APP_ROUTES.customers.url,
        },
      ],
    },
  ],
};

export function AppSidebar({
  sendTitle,
  userEmail,
  logout,
  ...props
}: SidebarProps) {
  const location = useLocation();
  const currentPath = location.pathname;

  React.useEffect(() => {
    const currentItem = data.navMain
      .flatMap((group) => group.items)
      .find((item) => item.url === currentPath);

    if (currentItem) {
      sendTitle(currentItem.title);
    } else {
      sendTitle(data.header[0].title);
    }
  }, [currentPath]);

  return (
    <Sidebar {...props}>
      <SidebarHeader className="flex h-16 shrink-0 px-4 justify-center">
        <h1 className="font-bold">{data.header[0].title}</h1>
      </SidebarHeader>
      <SidebarContent>
        {/* We create a SidebarGroup for each parent. */}
        {data.navMain.map((item) => (
          <SidebarGroup key={item.title}>
            <SidebarGroupLabel>{item.title}</SidebarGroupLabel>
            <SidebarGroupContent>
              <SidebarMenu>
                {item.items.map((childItem) => (
                  <SidebarMenuItem key={childItem.title}>
                    <SidebarMenuButton
                      asChild
                      isActive={currentPath === childItem.url}
                    >
                      <Link to={childItem.url}>{childItem.title}</Link>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                ))}
              </SidebarMenu>
            </SidebarGroupContent>
          </SidebarGroup>
        ))}
      </SidebarContent>
      <SidebarFooter>
        <NavUser
          user={{
            email: userEmail,
          }}
          logout={logout}
        />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  );
}
