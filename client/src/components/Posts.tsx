import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { useState } from "react";
import { createPost, getPosts } from "../api/posts"

type PostEdit = Parameters<typeof createPost>[0];

export const Posts = () => {
  const queryClient = useQueryClient();

  const query = useQuery({ queryKey: ['posts'], queryFn: getPosts });

  const mutation = useMutation({
    mutationFn: createPost,
    onSuccess: () => {
      queryClient.invalidateQueries(['posts']);
      setEditPost({ title: '', content: '' });
    },
  });

  const [editPost, setEditPost] = useState<PostEdit>({
    title: '',
    content: '',
  });

  const setEditPostTitle = (title: string) => {
    setEditPost({ ...editPost, title });
  };

  const setEditPostContent = (content: string) => {
    setEditPost({ ...editPost, content });
  };

  const addNewPost = () => {
    mutation.mutate(editPost);
  };

  return (
    <div>
      <ul>
        {query.data?.map(todo => (
          <li key={todo.id}>
            <h3>{todo.title}</h3>
            <p>{todo.content}</p>
          </li>
        ))}
      </ul>

      <div className="flex flex-col gap-1">
        <label htmlFor="post-title">Title:</label>
        <input id="post-title" className="text-black" type="text" value={editPost?.title} onChange={(e) => setEditPostTitle(e.target.value)} />

        <label htmlFor="post-content">Content</label>
        <input id="post-content" className="text-black" type="text" value={editPost?.content} onChange={(e) => setEditPostContent(e.target.value)} />

        <button onClick={addNewPost}>
          Add Post
        </button>
      </div>
    </div>
  )
}
