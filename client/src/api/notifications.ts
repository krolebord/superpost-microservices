import { FetchQueryOptions, QueryOptions } from "@tanstack/react-query";
import { authKey } from "./auth";
import { typedFetch } from "./helpers";

const notificationsApiUrl = '/api/notifications';
const notifierApiUrl = '/api/notifier';

export const notificationsSSEUrl = `${notifierApiUrl}/new`;

export const ContextType = {
  'post-create': 'Post',
} as const;

export type ContextType = keyof typeof ContextType;

export type NotificationContext = {
  contextType?: ContextType;
  contextId?: string;
}

export type Notification = {
  id: string;
  sentAt: string;
  readAt?: string;
  message: string;
} & (NotificationContext | undefined);


export const getNotifications = () => {
  return typedFetch<Notification[]>(notificationsApiUrl);
}

export const notificationsQuery = () => ({
  queryKey: [authKey, 'notifications'],
  queryFn: getNotifications,
}) satisfies QueryOptions;



export const markNotificationsRead = () => {
  return typedFetch<void>(`${notificationsApiUrl}/mark-read`, { method: 'POST' });
}
