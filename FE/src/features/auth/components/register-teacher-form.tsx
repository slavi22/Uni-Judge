import { cn } from "@/lib/utils.ts";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form.tsx";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";
import { Input } from "@/components/ui/input.tsx";
import PasswordInput from "@/components/inputs/password-input.tsx";
import { Button } from "@/components/ui/button.tsx";
import { Link, useNavigate } from "react-router";
import { type ComponentProps, useEffect, useRef } from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRegisterTeacherMutation } from "@/features/auth/api/auth-api.ts";

const formSchema = z
  .object({
    email: z.string().email({ message: "Invalid email address." }),
    password: z.string().min(1, { message: "Password cannot be empty." }), //TODO: enforce stricter password policy that will match the BE's policy
    confirmPassword: z
      .string()
      .min(1, { message: "Password cannot be empty." }),
    teacherSecret: z
      .string()
      .min(1, { message: "Teacher secret cannot be empty." }),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match.",
    path: ["confirmPassword"],
  });

export default function RegisterTeacherForm({
  className,
  ...props
}: ComponentProps<"div">) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: "",
      teacherSecret: "",
    },
  });
  const navigate = useNavigate();
  const containerRef = useRef<HTMLDivElement>(null);
  const [registerTeacher, { error, isLoading }] = useRegisterTeacherMutation();

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    const response = await registerTeacher({
      email: formData.email,
      password: formData.password,
      secret: formData.teacherSecret,
    });
    if (!response.error) {
      navigate("/login");
    }
  }

  useEffect(() => {
    containerRef.current?.focus();
  }, [error]);

  return (
    <div
      ref={containerRef}
      className={cn("flex flex-col gap-6", className)}
      {...props}
    >
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">Register as a Teacher</CardTitle>
          <CardDescription>Create a new account</CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            {isFetchBaseQueryError(error) && (
              <p className="mb-6 text-destructive">{error.data.detail}</p>
            )}
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
              <FormField
                name="email"
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>E-mail</FormLabel>
                    <FormControl>
                      <Input {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                name="password"
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Password</FormLabel>
                    <FormControl>
                      <PasswordInput {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                name="confirmPassword"
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Confirm Password</FormLabel>
                    <FormControl>
                      <PasswordInput {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                name="teacherSecret"
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Teacher Secret</FormLabel>
                    <FormControl>
                      <PasswordInput {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <Button
                className="w-full cursor-pointer mt-3"
                disabled={isLoading}
              >
                {isLoading ? "Submitting..." : "Register"}
              </Button>
            </form>
          </Form>
          <CardFooter className="flex justify-center border-t p-4 !pt-3">
            <p className="text-sm text-center text-muted-foreground">
              Already have an account?{" "}
              <Link
                to="/login"
                className="text-primary underline underline-offset-4 hover:text-primary/90 font-medium"
              >
                Sign in
              </Link>
            </p>
          </CardFooter>
        </CardContent>
      </Card>
    </div>
  );
}
