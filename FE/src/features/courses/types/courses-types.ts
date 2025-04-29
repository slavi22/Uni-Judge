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
