import { z } from "zod";
import { type ComponentProps, useEffect, useRef } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
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
import { Input } from "@/components/ui/input.tsx";
import { Button } from "@/components/ui/button.tsx";
import { Link, useNavigate } from "react-router";
import PasswordInput from "@/components/inputs/password-input.tsx";
import GoogleSigninProviderButton from "@/features/auth/components/google-signin-provider-button.tsx";
import { useRegisterMutation } from "@/features/auth/api/auth-api.ts";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";

const formSchema = z
  .object({
    email: z.string().email({ message: "Invalid email address." }),
    password: z.string().min(1, { message: "Password cannot be empty." }), //TODO: enforce stricter password policy that will match the BE's policy
    confirmPassword: z
      .string()
      .min(1, { message: "Password cannot be empty." }),
  })
  // confirm password => https://stackoverflow.com/a/76160363
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match.",
    path: ["confirmPassword"],
  });
// if we want to mark the password and confirm password as invalid we need another refine (warning: it will make the field selection clunky, for e.g if confirm password doesnt match it will hover password as being invalid and vice versa)
/*.refine((data) => data.password === data.confirmPassword, {
  message: "Passwords don't match.",
  path: ["confirmPassword"],
});*/

export default function RegisterForm({
  className,
  ...props
}: ComponentProps<"div">) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  const navigate = useNavigate();
  const containerRef = useRef<HTMLDivElement>(null);
  const [registerUser, { error, isLoading }] = useRegisterMutation();

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    const response = await registerUser({
      email: formData.email,
      password: formData.password,
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
          <CardTitle className="text-2xl">Register</CardTitle>
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
                control={form.control} // this helps RHF (react-hook-form) manage the state {errors, values, touched status etc) of controlled components, we dont need to specify it here since <Form {...form} /> passes it implicitly
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
              <Button
                className="w-full cursor-pointer mt-3"
                disabled={isLoading}
              >
                {isLoading ? "Submitting..." : "Register"}
              </Button>
              <div className="mb-4 after:border-border relative text-center text-sm after:absolute after:inset-0 after:top-1/2 after:z-0 after:flex after:items-center after:border-t">
                <span className="bg-card text-muted-foreground relative z-10 px-2">
                  Or register with
                </span>
              </div>
              <GoogleSigninProviderButton
                buttonText="Register with Google"
                className="mb-6"
              />
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
