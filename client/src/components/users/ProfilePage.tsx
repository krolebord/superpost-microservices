import { useQuery } from "@tanstack/react-query";
import { useLoaderData, useParams } from "react-router-dom";
import { InferLoaderData } from "../../api/helpers";
import { userNewTimelineQuery } from "../../api/timeline";
import { profileLoader, profileQuery } from "../../api/users";
import { QueryLoader } from "../QueryLoader";
import { PostList } from "../posts/PostList";
import { ProfileCard } from "./ProfileCard";
import { cardHorizontalMargin } from "../../common-styles";

export const ProfilePage = () => {
  const initialData = useLoaderData() as InferLoaderData<ReturnType<typeof profileLoader>>;
  const params = useParams();
  const { data: profile } = useQuery({
    ...profileQuery(params.userId!),
    initialData,
  });

  const postsQuery = userNewTimelineQuery(profile.id);

  return (<div className={cardHorizontalMargin}>
    <ProfileCard profile={profile} />

    <h2 className="mx-2 xs:mx-4 md:mx-8 lg:mx-12 text-lg mt-4 mb-2">Last Posts</h2>
    <QueryLoader query={postsQuery} loaded={posts =>
      <PostList posts={posts} className="mx-2 xs:mx-4 md:mx-8 lg:mx-12" />
    } />
  </div>);
}
