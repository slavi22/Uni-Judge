import { BookOpen, BookType, Notebook, ShieldUser } from "lucide-react";

export const studentNav = {
  navItems: [
    {
      title: "Student",
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
    },
  ],
};

export const teacherNav = {
  navItems: [
    {
      title: "Student",
      icon: Notebook,
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
    },
    {
      title: "Teacher",
      icon: BookType,
      isActive: true,
      items: [
        {
          title: "Create a new course",
          url: "/courses/create-new-course",
        },
        {
          title: "Create a new problem",
          url: "/problems/create-new-problem",
        },
      ],
    },
  ],
};

export const adminNav = {
  navItems: [
    {
      title: "Student",
      icon: Notebook,
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
    },
    {
      title: "Teacher",
      icon: BookType,
      items: [
        {
          title: "Add a new course",
          url: "/courses/add-new-course",
        },
        {
          title: "Create a new problem",
          url: "/problems/create-new-problem",
        },
      ]
    },
    {
      title: "Admin",
      icon: ShieldUser,
      isActive: true,
      items: [
        {
          title: "Admin stuff",
          url: "#",
        },
      ]
    },
  ],
};
