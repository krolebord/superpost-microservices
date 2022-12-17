const authApiUrl = '/api/auth';

export const authKey = 'auth';

export type SignInCredentials = {
  userNameOrEmail: string;
  password: string;
};
export const signIn = (credentials: SignInCredentials) => {
  return fetch(`${authApiUrl}/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(credentials),
  });
};

export type SignUpCredentials = {
  userName: string;
  email: string;
  password: string;
};
export const signUp = (credentials: SignUpCredentials) => {
  return fetch(`${authApiUrl}/register`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(credentials),
  });
}

export const signOut = () => {
  return fetch(`${authApiUrl}/logout`, { method: 'POST' });
};
