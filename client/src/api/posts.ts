type Post = {
  id: string;
  title: string;
  content: string;
}

export const postsApiUrl = "/api/posts";

export const getPosts = () => {
  return fetch(postsApiUrl).then((res) => res.json()) as Promise<Post[]>;
}

export const createPost = (post: Omit<Post, 'id'>) => {
  return fetch(postsApiUrl, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(post),
  }).then((res) => res.ok ? res.json() : Promise.reject(res)) as Promise<string>;
}

