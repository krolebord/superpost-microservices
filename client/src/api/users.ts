import { QueryClient, QueryOptions } from "@tanstack/react-query";
import { LoaderFunction, redirect } from "react-router-dom";
import { typedFetch } from "./helpers";

const usersApiUrl = '/api/users'

export type User = {
  id: string;
  name: string;
}

export type CurrentUser = {
  id: string;
  name: string;
  subscribersCount: number;
  subscribedToCount: number;
}

export type Profile = {
  id: string;
  name: string;
  subscribersCount: number;
  subscribedToCount: number;
}

export const getMyProfile = () => {
  return typedFetch<Profile>(`${usersApiUrl}/profile/me`)
    .catch(() => Promise.resolve(null));
};

const getProfile = async (userName: string) => {
  return typedFetch<Profile>(`${usersApiUrl}/profile/${userName}`);
}

export const profileQuery = (userId: string) => ({
  queryKey: ['profile', userId],
  queryFn: () => getProfile(userId),
}) satisfies QueryOptions;

export const profileLoader = (client: QueryClient) => 
  (async ({ params }) => {
    if (!params.userId) {
      return redirect('/404');
    }
    const query = profileQuery(params.userId);
    return client.getQueryData(query.queryKey) as Profile || await client.fetchQuery(query);
  }) satisfies LoaderFunction;



export const checkIsSubscribed = (userId: string) => {
  return typedFetch<boolean>(`${usersApiUrl}/is-subscribed-to/${userId}`);
}

export const isSubscribedQuery = (userId: string) => ({
  queryKey: ['is-subscribed', userId],
  queryFn: () => checkIsSubscribed(userId),
}) satisfies QueryOptions;



export const subscribe = (userId: string) => {
  return fetch(`${usersApiUrl}/subscribe?userId=${userId}`, {
    method: 'POST',
  });
};

export const unsubscribe = (userId: string) => {
  return fetch(`${usersApiUrl}/unsubscribe?userId=${userId}`, {
    method: 'POST',
  });
};
