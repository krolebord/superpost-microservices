import clsx from "clsx";
import { FC } from "react";
import { TimelinePost } from "../../api/timeline";    
import { PostCard } from "./PostCard";

type PostListProps = {
  posts: TimelinePost[];
  className?: string;
};

export const PostList: FC<PostListProps> = ({ posts, className }) => {
  return (<div className={clsx('flex flex-col gap-2', className)}>
    {posts.map((post) =>
      <PostCard post={post} />
    )}
  </div>);
}
