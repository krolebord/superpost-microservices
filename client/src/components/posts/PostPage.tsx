import { useQuery } from "@tanstack/react-query";
import { useLoaderData, useParams } from "react-router-dom";
import { InferLoaderData } from "../../api/helpers";
import { postTimelineLoader, postTimelineQuery } from "../../api/timeline";
import { cardHorizontalMargin } from "../../common-styles";
import { PostCard } from "./PostCard";
import { PostEditor } from "./PostEditor";

export const PostPage = () => {
  const initialData = useLoaderData() as InferLoaderData<ReturnType<typeof postTimelineLoader>>;
  const params = useParams();
  const { data: post } = useQuery({
    ...postTimelineQuery(params.postId!),
    initialData,
  });

  return (<div className={cardHorizontalMargin}>
    <div className="mx-2 xs:mx-3 sm:mx-4 md:mx-8 lg:mx-12 flex flex-col gap-2">
      {post.parentPosts && post.parentPosts.map(parentPost =>
        <PostCard key={parentPost.id} post={parentPost} />
      )}
    </div>
    <PostCard className="my-2" post={{...post, subPostsCount: post.subPosts.length }} />
    <div className="mx-2 xs:mx-3 sm:mx-4 md:mx-8 lg:mx-12 flex flex-col gap-2">
      <PostEditor parentPostId={post.id} />
      {post.subPosts.map(subPost =>
        <PostCard key={subPost.id} post={subPost} />
      )}
    </div>
  </div>);
}
