import { FetchQueryOptions, QueryOptions, UseQueryOptions } from "@tanstack/react-query";
import { typedFetch } from "./helpers";

type NewPost = {
  content: string;
  parentPostId?: string;
}

export const postsApiUrl = "/api/posts";

export const createPost = (post: NewPost) => {
  return typedFetch<string>(postsApiUrl, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(post),
  });
}


export const getHaltStatus = () => {
  return typedFetch<boolean>(`${postsApiUrl}/halt`);
}

export const haltQuery = {
  queryFn: getHaltStatus,
  queryKey: ['halt'],
} satisfies QueryOptions;


export const toggleHalt = () => {
  return fetch(`${postsApiUrl}/halt`, { method: 'POST' });
}
