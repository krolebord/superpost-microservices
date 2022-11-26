import { useQuery } from "@tanstack/react-query";
import { Link, useLoaderData } from "react-router-dom";
import { InferLoaderData } from "../../api/helpers";
import { homeTimelineLoader, homeTimelineQuery, newTimelineLoader, newTimelineQuery } from "../../api/timeline";
import { cardHorizontalMargin } from "../../common-styles";
import { PostEditor } from "../posts/PostEditor";
import { PostList } from "../posts/PostList";

export const HomeTimelinePage = () => {
  const initialData = useLoaderData() as InferLoaderData<ReturnType<typeof homeTimelineLoader>>;
  const { data: posts } = useQuery({
    ...homeTimelineQuery(),
    initialData,
  });

  return <div className={cardHorizontalMargin}>
    <PostEditor />
    <h1 className="text-lg mt-4 pb-2 text-gray-300">Your subscriptions</h1>
    {posts.length === 0 && <div className="my-8 text-lg text-center text-gray-300">
      <p>There is nothing to show here {';('}</p>
      <p>Go to <Link to="/new" className="text-purple-400">New page</Link> to view latest posts</p>
      <p>And subscribe to new users</p> 
    </div>}
    {posts.length > 0 && <>
      <PostList posts={posts} />
    </>}
  </div>;
}

export const NewTimelinePage = () => {
  const initialData = useLoaderData() as InferLoaderData<ReturnType<typeof newTimelineLoader>>;
  const { data: posts } = useQuery({
    ...newTimelineQuery(),
    initialData,
  });

  return <div className={cardHorizontalMargin}>
    <h1 className="text-lg pb-2 text-gray-300">New Posts</h1>
    <PostList posts={posts} />
  </div>;
}
