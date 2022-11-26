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

