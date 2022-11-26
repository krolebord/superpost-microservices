import clsx from "clsx";
import { InputHTMLAttributes } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "./AuthProvider";

type AuthInputProps = {
  id: string;
  label: string;
  className?: string;
} & InputHTMLAttributes<HTMLInputElement>;
const AuthInput = (props: AuthInputProps) => {
  const { id, label, className, ...inputProps } = props;

  return (
    <div className="flex flex-col gap-1">
      <label htmlFor={id}>{label}</label>
      <input
        id={id}
        className={clsx('bg-slate-500/40 disabled:bg-slate-500/30 rounded-md [line-height:2] px-1', className)}
        {...inputProps}
      />
    </div>
  );
};

export const SignInForm = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { signIn, loading } = useAuth();

  const from = location.state?.from?.pathname || "/home";

  const onSignIn = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const formData = new FormData(event.currentTarget);
    const userNameOrEmail = formData.get('email') as string;
    const password = formData.get('password') as string;

    signIn({ userNameOrEmail, password }, () => navigate(from, { replace: true }));
  };

  return (
    <form
      className="flex flex-col gap-2 bg-slate-700/50 xs:mx-8 sm:mx-10 md:mx-16 rounded-xl px-6 py-4"
      onSubmit={onSignIn}
    >
      <AuthInput
        id="email-or-username-input"
        label="Username or Email"
        type="text"
        name="email"
        autoComplete="email"
        disabled={loading}
      />

      <AuthInput
        id="password-input"
        label="Password"
        type="password"
        name="password"
        autoComplete="current-password"
        disabled={loading}
      />

      <input type="submit" value="Sign in" disabled={loading} className="cursor-pointer disabled:text-white/40"/>
    </form>
  );
};

export const SignUpForm = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { signUp, loading } = useAuth();

  const from = location.state?.from?.pathname || '/home';

  const onSignUp = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const formData = new FormData(event.currentTarget);
    const userName = formData.get('username') as string;
    const email = formData.get('email') as string;
    const password = formData.get('password') as string;

    signUp({ userName, email, password }, () => navigate(from, { replace: true }));
  };

  return (
    <form
      className="flex flex-col gap-2 bg-slate-700/50 xs:mx-8 sm:mx-10 md:mx-16 rounded-xl px-6 py-4"
      onSubmit={onSignUp}
    >
      <AuthInput
        id="username-input"
        label="Username"
        type="text"
        name="username"
        autoComplete="username"
        disabled={loading}
      />

      <AuthInput
        id="email-input"
        label="Email"
        type="email"
        name="email"
        autoComplete="email"
        disabled={loading}
      />

      <AuthInput
        id="password-input"
        label="Password"
        type="password"
        name="password"
        autoComplete="current-password"
        disabled={loading}
      />

      <input type="submit" value="Sign up" disabled={loading} className="cursor-pointer  disabled:text-white/40"/>
    </form>
  );
};
