import clsx from "clsx";
import { FC } from "react";
import { Link } from "react-router-dom";
import { TimelinePost } from "../../api/timeline";
import { UserAvatar } from "../users/UserAvatar";
import { UserDisplay } from "../users/UserDisplay";

type Props = {
  post: TimelinePost;
  className?: string;
};

export const PostCard: FC<Props> = ({ post, className }) => {
  return (<div className={clsx('block bg-slate-700/50 rounded-xl', className)}>
    <div className="p-3 border-b border-b-slate-600 flex flex-row justify-between items-center">
      <UserDisplay avatarSize="md" avatarClassName="ml-3 text-xl" userId={post.user.id} userName={post.user.name} />
      <span>{new Date(post.createdAt).toLocaleString()}</span>
    </div>
    <Link to={`/post/${post.id}`} className="block hover:bg-slate-700/70 rounded-b-xl p-3">
      <p>
        {post.content}
      </p>
      <p className="text-end">{post.subPostsCount} {post.subPostsCount == 1 ? 'subpost' : 'subposts'}</p>
    </Link>
  </div>)
}
