import { type ComponentProps, useState } from "react";
import { Input } from "@/components/ui/input.tsx";
import { Eye, EyeOff } from "lucide-react";

export default function PasswordInput({ ...props }: ComponentProps<"input">) {
  const [inputType, setInputType] = useState<"password" | "text">("password");
  return (
    <span className="flex items-center relative">
      <Input type={inputType} {...props} />
      <button
        type="button"
        className="absolute right-1 cursor-pointer"
        onClick={() =>
          setInputType(inputType === "password" ? "text" : "password")
        }
      >
        {inputType === "password" ? <Eye className="h-6"/> : <EyeOff className="h-6"/>}
      </button>
    </span>
  );
}
