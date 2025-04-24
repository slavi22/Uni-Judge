import CreateNewCourseForm from "@/features/courses/components/create-new-course-form.tsx";

export default function CreateNewCoursePage() {
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <CreateNewCourseForm />
      </div>
    </div>
  );
}
