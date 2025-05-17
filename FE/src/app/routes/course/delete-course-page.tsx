import { useGetAllCoursesQuery } from "@/features/courses/api/course-api.ts";
import { Card, CardContent } from "@/components/ui/card.tsx";
import AlertDialogDeleteProblem from "@/components/alert-dialogs/alert-dialog-delete-problem.tsx";

export default function DeleteCoursePage() {
  const { data, refetch } = useGetAllCoursesQuery(undefined, {
    refetchOnMountOrArgChange: true,
  });

  return (
    <div className="container mx-auto py-8">
      <div className="mb-6 w-fit mx-auto text-center">
        <h1 className="text-2xl font-bold mb-2 text-left">Delete Courses</h1>
        <p className="text-muted-foreground">
          Here is a list of all courses. As an admin, you can delete any course
          from the system.
        </p>
      </div>

      <div className="flex flex-col space-y-4 w-1/3 mx-auto">
        {data?.map((course) => (
          <Card key={course.courseId} className="w-full">
            <CardContent className="flex justify-between items-center py-4">
              <div className="font-medium">{course.name}</div>
              <AlertDialogDeleteProblem
                courseId={course.courseId}
                refetchParent={refetch}
              />
            </CardContent>
          </Card>
        ))}

        {!data?.length && (
          <div className="text-center py-10">
            <p className="text-muted-foreground">
              No courses found in the system
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
