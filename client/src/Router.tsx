import { createBrowserRouter, Navigate, RouteObject } from "react-router-dom";
import { profileLoader, profileQuery } from "./api/users";
import { SignInForm, SignUpForm } from "./components/auth/AuthForms";
import { AuthLayout } from "./components/auth/AuthLayout";
import { RequireAuth } from "./components/auth/AuthProvider";
import { NotFound } from "./components/NotFound";
import { RootLayout } from "./components/RootLayout";
import { PostList } from "./components/posts/PostList";
import { ProfilePage } from "./components/users/ProfilePage";
import { queryClient } from "./query-client";
import { homeTimelineLoader, newTimelineLoader, postTimelineLoader } from "./api/timeline";
import { PostPage } from "./components/posts/PostPage";
import { HomeTimelinePage, NewTimelinePage } from "./components/timeline/TimelinePages";

const authenticatedRoutes: RouteObject[] = [
  {
    path: '/',
    element: <Navigate to="/home" replace={true} />,
  },
  {
    path: '/home',
    loader: homeTimelineLoader(queryClient),
    element: <HomeTimelinePage />
  },
  {
    path: '/new',
    loader: newTimelineLoader(queryClient),
    element: <NewTimelinePage />
  },
  {
    path: '/profile/:userId',
    loader: profileLoader(queryClient),
    element: <ProfilePage />
  },
  {
    path: '/post/:postId',
    loader: postTimelineLoader(queryClient),
    element: <PostPage />
  }
];

export const router = createBrowserRouter([
  {
    path: '/',
    element: <RootLayout />,
    errorElement: <NotFound />,
    children: [
      {
        id: 'auth',
        element: <AuthLayout />,
        children: [
          {
            path: "/sign-in",
            element: <SignInForm />,
          },
          {
            path: "/sign-up",
            element: <SignUpForm />,
          },
        ]
      },
      {
        id: 'authenticated',
        element: <RequireAuth />,
        children: authenticatedRoutes
      }
    ]
  }
]);
