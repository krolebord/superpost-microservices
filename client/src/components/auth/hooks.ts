import { useQuery, useQueryClient } from "@tanstack/react-query";
import { SignInCredentials, SignUpCredentials, authKey, signIn, signUp, signOut } from "../../api/auth";
import { CurrentUser, myProfileQuery } from "../../api/users";

type AuthData = {
  loading: boolean;
  user: CurrentUser | null;
}

type AuthActions = {
  signIn: (credentials: SignInCredentials, callback?: VoidFunction) => void;
  signUp: (credentials: SignUpCredentials, callback?: VoidFunction) => void;
  signOut: (callback?: VoidFunction) => void;
}

export function useAuth(options?: { forceUser: false }): AuthData
export function useAuth(options: { forceUser: true }): AuthData & { user: CurrentUser }
export function useAuth(options?: { forceUser: boolean }) {
  const { data: profile, isSuccess, isInitialLoading: loading } = useQuery(myProfileQuery);

  const data: AuthData = {
    loading,
    user: isSuccess ? profile : null
  };

  if (!options?.forceUser) {
    return data;
  }
  if (!data.user) {
    throw new Error('User is not authenticated');
  }
  return data;
}

export const useAuthActions = (): AuthActions => {
  const client = useQueryClient();

  const handleAuthAction = (action: Promise<unknown>, callback?: VoidFunction) => action
    .then(() => client.invalidateQueries({
      queryKey: [authKey],
      refetchType: 'all'
    }))
    .then(() => setTimeout(() => callback?.(), 0));

  return {
    signIn: (credentials, callback) => handleAuthAction(signIn(credentials), callback),
    signUp: (credentials, callback) => handleAuthAction(signUp(credentials), callback),
    signOut: (callback) => handleAuthAction(signOut(), callback)
  };
}
