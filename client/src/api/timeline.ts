import { QueryClient, QueryOptions } from "@tanstack/react-query";
import { LoaderFunction, redirect } from "react-router-dom";
import { authKey } from "./auth";
import { typedFetch } from "./helpers";
import { User, authenticatedLoader } from "./users";

const timelineApiUrl = '/api/timeline';

export type TimelinePost = {
  id: string;
  content: string;
  createdAt: string;
  subPostsCount: number;
  user: User;
};

export type FullTimelinePost = {
  id: string;
  content: string;
  createdAt: string;
  parentPosts?: TimelinePost[];
  subPosts: TimelinePost[];
  user: User;
}

export const getHomeTimeline = () => {
  return typedFetch<TimelinePost[]>(`${timelineApiUrl}/home`);
}

export const homeTimelineQuery = () => ({
  queryKey: [authKey, 'home-timeline'],
  queryFn: getHomeTimeline,
});

export const homeTimelineLoader = (client: QueryClient) => 
  authenticatedLoader(client, async () => {
    const query = homeTimelineQuery();
    return client.getQueryData(query.queryKey) as TimelinePost[] || await client.fetchQuery(query);
  });



export const getNewTimeline = () => {
  return typedFetch<TimelinePost[]>(`${timelineApiUrl}/new`);
}

export const newTimelineQuery = () => ({
  queryKey: ['new-timeline'],
  queryFn: getNewTimeline,
});

export const newTimelineLoader = (client: QueryClient) => 
  authenticatedLoader(client, async () => {
    const query = newTimelineQuery();
    return client.getQueryData(query.queryKey) as TimelinePost[] || await client.fetchQuery(query);
  });



export const getUserNewTimeline = (userId: string) => {
  return typedFetch<TimelinePost[]>(`${timelineApiUrl}/new/${userId}`);
}

export const userNewTimelineQuery = (userId: string) => ({
  queryKey: ['user-timeline', userId],
  queryFn: () => getUserNewTimeline(userId),
});



export const getPostTimeline = (postId: string) => {
  return typedFetch<FullTimelinePost>(`${timelineApiUrl}/post/${postId}`);
}

export const postTimelineQuery = (postId: string) => ({
  queryKey: ['post-timeline', postId],
  queryFn: () => getPostTimeline(postId),
});

export const postTimelineLoader = (client: QueryClient) => 
  authenticatedLoader(client, async ({ params }) => {
    if (!params.postId) {
      return redirect('/404');
    }
    const query = postTimelineQuery(params.postId);
    return client.getQueryData(query.queryKey) as FullTimelinePost || await client.fetchQuery(query);
  });
