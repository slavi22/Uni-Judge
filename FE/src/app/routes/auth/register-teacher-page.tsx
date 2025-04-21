import RegisterTeacherForm from "@/features/auth/components/register-teacher-form.tsx";

export default function RegisterTeacherPage() {
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <RegisterTeacherForm />
      </div>
    </div>
  );
}
