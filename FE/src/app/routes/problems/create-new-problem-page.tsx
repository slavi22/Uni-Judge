import CreateNewProblemForm from "@/features/problems/components/create-new-problem-form.tsx";

export default function CreateNewProblemPage() {
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <CreateNewProblemForm />
      </div>
    </div>
  );
}
