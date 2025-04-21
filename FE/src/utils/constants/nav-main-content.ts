import { BookOpen, BookType, Notebook, ShieldUser } from "lucide-react";

export const studentNav = {
  //TODO: maybe replace inside the navmain component ?? like fetch the user's stuff and dynamically generate the menu
  navItems: [
    {
      title: "Student",
      url: "#",
      icon: Notebook,
      isActive: true,
      items: [
        {
          title: "My Submissions",
          url: "#",
        },
        {
          title: "My Courses",
          url: "#",
        },
      ],
    },
    {
      title: "All Courses",
      icon: BookOpen,
      url: "#",
    },
  ],
};

export const teacherNav = {
  navItems: [
    {
      title: "Student",
      url: "#",
      icon: Notebook,
      isActive: true,
      items: [
        {
          title: "My Submissions",
          url: "#",
        },
        {
          title: "My Courses",
          url: "#",
        },
      ],
    },
    {
      title: "All Courses",
      icon: BookOpen,
      url: "#",
    },
    {
      title: "Teacher",
      icon: BookType,
      url: "#",
      items: [
        {
          title: "Teacher stuff",
          url: "#",
        },
      ]
    },
  ],
};

export const adminNav = {
  navItems: [
    {
      title: "Student",
      url: "#",
      icon: Notebook,
      isActive: true,
      items: [
        {
          title: "My Submissions",
          url: "#",
        },
        {
          title: "My Courses",
          url: "#",
        },
      ],
    },
    {
      title: "All Courses",
      icon: BookOpen,
      url: "#",
    },
    {
      title: "Teacher",
      icon: BookType,
      url: "#",
      items: [
        {
          title: "Teacher stuff",
          url: "#",
        },
      ]
    },
    {
      title: "Admin",
      icon: ShieldUser,
      url: "#",
      items: [
        {
          title: "Admin stuff",
          url: "#",
        },
      ]
    },
  ],
};
