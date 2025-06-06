import { cn } from "@/lib/utils.ts";
import { Button } from "@/components/ui/button.tsx";
import { Input } from "@/components/ui/input.tsx";
import { type ComponentProps } from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";
import { useLoginMutation } from "@/features/auth/api/auth-api.ts";
import { Link, useNavigate } from "react-router";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form.tsx";
import { isFetchBaseQueryError } from "@/utils/functions/is-fetch-base-query-error.ts";
import GoogleSigninProviderButton from "@/features/auth/components/google-signin-provider-button.tsx";
import PasswordInput from "@/components/inputs/password-input.tsx";

const formSchema = z.object({
  email: z.string().email({ message: "Invalid email address." }),
  password: z.string().min(1, { message: "Password cannot be empty." }), //TODO: enforce stricter password policy that will match the BE's policy
});

export function LoginForm({ className, ...props }: ComponentProps<"div">) {
  const [loginUser, { error, isLoading }] = useLoginMutation();
  const navigate = useNavigate();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  async function onSubmit(formData: z.infer<typeof formSchema>) {
    const result = await loginUser(formData);
    if (!result.error) {
      navigate("/");
    }
  }

  // react-hook-form quick guide => https://www.youtube.com/watch?v=cc_xmawJ8Kg
  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">Login</CardTitle>
          <CardDescription>
            Enter your credentials below to login to your account
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            {isFetchBaseQueryError(error) && (
              <p className="mb-6 text-destructive">{error.data.detail}</p>
            )}
            <form onSubmit={form.handleSubmit(onSubmit)}>
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem className="mb-8">
                    <FormLabel>Email</FormLabel>
                    <FormControl>
                      <Input placeholder="email@domain.com" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem className="mb-8 grid gap-2">
                    <div className="flex items-center">
                      <FormLabel>Password</FormLabel>
                      <a
                        href="#"
                        className="ml-auto inline-block text-sm underline-offset-4 hover:underline"
                      >
                        {/*TODO: add password reset*/}
                        Forgot your password?
                      </a>
                    </div>
                    <FormControl className="flex items-center">
                      <PasswordInput placeholder="********" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <Button
                className="w-full cursor-pointer mb-4"
                disabled={isLoading}
              >
                {isLoading ? "Submitting..." : "Log in"}
              </Button>
              <div className="mb-4 after:border-border relative text-center text-sm after:absolute after:inset-0 after:top-1/2 after:z-0 after:flex after:items-center after:border-t">
                <span className="bg-card text-muted-foreground relative z-10 px-2">
                  Or continue with
                </span>
              </div>
              <GoogleSigninProviderButton
                buttonText="Login with Google"
                className="mb-3"
              />
            </form>
          </Form>
          <CardFooter className="flex flex-col w-full px-0">
            <div className="text-center text-sm">
              <p className="text-sm text-center text-muted-foreground">
                Don&apos;t have an account?{" "}
                <Link
                  to="/register"
                  className="text-primary underline underline-offset-4 hover:text-primary/90 font-medium"
                >
                  Sign up
                </Link>
              </p>
            </div>
            <div className="w-full relative mt-2 text-center text-sm after:absolute after:inset-0 after:top-1/2 after:z-0 after:flex after:items-center after:border-t after:border-border">
              <span className="bg-card text-muted-foreground relative z-10 px-2">
                Or if you&apos;re a teacher
              </span>
            </div>
            <div className="mt-2 text-center text-sm">
              <Link
                to="/register-teacher"
                className="text-primary underline underline-offset-4 hover:text-primary/90 font-medium"
              >
                Register as a teacher
              </Link>
            </div>
          </CardFooter>
        </CardContent>
      </Card>
    </div>
  );
}
