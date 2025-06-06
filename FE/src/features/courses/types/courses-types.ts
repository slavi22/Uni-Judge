export type NewCourseDto = {
  courseId: string;
  name: string;
  description: string;
  password?: string | null;
};

export type TeacherCoursesDto = {
  courseId: string;
  name: string;
  description: string;
}

export type AllCoursesDto = {
  courseId: string;
  name: string;
  isPasswordProtected: boolean;
  userIsEnrolled: boolean;
}

export type SignUpForCourseDto = {
  courseId: string;
  password?: string;
}

export type EnrolledCoursesDto = {
  courseId: string;
  name: string;
  description: string;
}

export type CourseProblemDto = {
  problemId: string;
  name: string;
  description: string;
}
