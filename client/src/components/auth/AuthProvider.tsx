import { useQuery, useQueryClient } from "@tanstack/react-query";
import { createContext, ReactNode, useContext } from "react";
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { signIn, signUp, signOut, SignInCredentials, SignUpCredentials } from "../../api/auth";
import { CurrentUser, getMyProfile } from "../../api/users";

type AuthContext = {
  loading: boolean;
  user: CurrentUser | null;
  signIn: (credentials: SignInCredentials, callback?: VoidFunction) => void;
  signUp: (credentials: SignUpCredentials, callback?: VoidFunction) => void;
  signOut: (callback?: VoidFunction) => void;
}

const AuthContext = createContext<AuthContext>(null!);

export const authKey = 'auth';

export function useAuth(options?: { forceUser: false }): AuthContext
export function useAuth(options: { forceUser: true }): AuthContext & { user: CurrentUser }
export function useAuth(options?: { forceUser: boolean }) {
  const context = useContext(AuthContext);
  if (!options?.forceUser) {
    return context;
  }
  if (!context.user) {
    throw new Error('User is not authenticated');
  }
  return context;
}

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const client = useQueryClient();
  const { data: profile, isSuccess, isInitialLoading } = useQuery({
    queryFn: getMyProfile,
    queryKey: [authKey, 'current-user'],
    retry: false,
  });

  const handleAuthAction = (action: Promise<unknown>, callback?: VoidFunction) => action
    .then(() => client.invalidateQueries({
      queryKey: [authKey],
      refetchType: 'all'
    }))
    .then(() => setTimeout(() => callback?.(), 0));

  const value: AuthContext = {
    loading: isInitialLoading,
    user: isSuccess ? profile : null,
    signIn: (credentials, callback) => handleAuthAction(signIn(credentials), callback),
    signUp: (credentials, callback) => handleAuthAction(signUp(credentials), callback),
    signOut: (callback) => handleAuthAction(signOut(), callback)
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export const RequireAuth = () => {
  const auth = useAuth();
  const location = useLocation();

  if (auth.loading) return null;

  if (!auth.user) {
    return <Navigate to="/sign-up" state={{ from: location }} replace />;
  }

  return <Outlet />;
}
