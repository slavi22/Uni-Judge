import { LoaderCircle } from "lucide-react";

export default function LoadingSpinner() {
  return (
    <div className="flex flex-col justify-center items-center h-screen m-auto">
      <LoaderCircle
        className="animate-spin w-8 h-8 sm:w-16 sm:h-16 2xl:w-32 2xl:h-32"
        width={undefined}
        height={undefined}
      />
      <p className="text-xs sm:text-sm md:text-base xl:text-xl 2xl:text-2xl">
        Loading user profile...
      </p>
    </div>
  );
}
