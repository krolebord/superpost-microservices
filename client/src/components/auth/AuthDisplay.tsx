import clsx from "clsx";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { UserDisplay } from "../users/UserDisplay";
import { useAuth } from "./AuthProvider";

export const SignInLink = ({ className }: { className?: string }) => {
  const location = useLocation();
  const from = location.state?.from ?? location;
  return <Link to={'/sign-in'} state={{ from }} className={className} >Sign in</Link>;
};

export const SignUpLink = ({ className }: { className?: string }) => {
  const location = useLocation();
  const from = location.state?.from ?? location;
  return <Link to={'/sign-up'} state={{ from }} className={className} >Sign up</Link>;
};

const ProfileDisplay = ({ className }: { className?: string }) => {
  const navigate = useNavigate();
  const { user, signOut } = useAuth({ forceUser: true });

  const onSingOut = () => {
    signOut(() => navigate('/'));
  }

  return (<div className={clsx('relative', className)}>
    <UserDisplay avatarSize="sm" avatarClassName="peer-hover:invisible" userId={user.id} userName={user.name} />
    <span className="peer ml-2">X</span>
    <button
      onClick={onSingOut}
      className="absolute left-0 right-0 top-0 bottom-0 hidden bg-slate-800 hover:block peer-hover:block rounded-lg border-purple-500/60 border-2 px-1"
    >Sign out</button>
  </div>);
};

const LoginDisplay = ({ className }: { className?: string }) => {
  

  return (<div className={clsx('flex gap-3 items-center', className)}>
      <SignInLink />
      <SignUpLink className="bg-purple-500/70 hover:bg-purple-500/80 rounded-lg px-1 sm:px-4 py-0 sm:py-1" />
  </div>);
}

export const AuthDisplay = ({ className }: { className?: string }) => {
  const { user, loading } = useAuth();

  if (loading) {
    return <></>;
  }

  return (<>
    {user ? <ProfileDisplay className={className} /> : <LoginDisplay className={className} />}
  </>);
}
