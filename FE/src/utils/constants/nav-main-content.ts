import { BookOpen, Notebook } from "lucide-react";
export const nav = {
  //TODO: maybe replace inside the navmain component ?? like fetch the user's stuff and dynamically generate the menu
  main: [
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