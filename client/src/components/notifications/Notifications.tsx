import { Icon } from '@iconify/react';
import { useNotifications } from './use-notifications';
import { ContextType, Notification, markNotificationsRead, notificationsQuery } from '../../api/notifications'
import { FC, useState } from 'react';
import clsx from 'clsx';
import { formatRelativeTime } from '../../helpers/time-helper';
import { Link } from 'react-router-dom';
import { useMutation, useQueryClient } from '@tanstack/react-query';

const NotificationDisplay: FC<{ notification: Notification, className?: string }> = ({ notification, className }) => {
  const clickable = notification.contextType !== null;
  const wrapperClass = clsx('p-2 block', className, { 'bg-slate-700/60': !notification.readAt, 'hover:bg-slate-700': clickable });

  const content = <>
    <p className="text-sm text-white/70 flex justify-between">
      <span>
        {notification.contextType ? ContextType[notification.contextType] : 'Notification'}
      </span>
      <span>
        {formatRelativeTime(notification.sentAt)}
      </span>
    </p>
    <p>{notification.message}</p>
  </>;

  return clickable
    ? <Link to={`/post/${notification.contextId}`} className={wrapperClass}>{content}</Link>
    : <div className={wrapperClass}>{content}</div>;
};

export const Notifications: FC = () => {
  const [opened, setOpened] = useState(false);
  const notifications = useNotifications();
  const hasUnread = notifications.some(x => !x.readAt);

  const queryClient = useQueryClient();
  const markReadMutation = useMutation({
    mutationFn: () => markNotificationsRead(),
    onMutate: async () => {
      const key = notificationsQuery().queryKey;

      await queryClient.cancelQueries(key);
      const previousNotifications = queryClient.getQueryData<Notification[]>(key);

      if (previousNotifications) {
        queryClient.setQueryData<Notification[]>(key, old => old?.map(x => ({
          ...x,
          readAt: new Date().toString()
        })));
      }

      return { previousNotifications }
    },
    onError: (_, __, context) => {
      const key = notificationsQuery().queryKey;
      queryClient.setQueryData(key, context?.previousNotifications)
    },
    onSettled: () => {
      const key = notificationsQuery().queryKey;
      queryClient.invalidateQueries({ queryKey: key })
    },
  })

  return <div>
    <button className="mt-2 relative" onClick={() => setOpened(x => !x)} >
      <Icon icon={opened ? 'material-symbols:notifications' : 'material-symbols:notifications-outline'} fontSize="20px" />
      {hasUnread && <span className="absolute top-0.5 right-0.5 w-2 h-2 bg-purple-500 rounded-full" />}
    </button>
    {opened && <div className="max-h-[70vh] md:max-h-[40vh] overflow-auto bg-slate-800 shadow-lg w-80 absolute top-10 right-2 rounded-b-lg border border-t-0 border-slate-500">
      <p className="px-2 py-3 flex justify-between">
        <span className="font-bold" >Notifications</span>
        {hasUnread && <button className="bg-purple-500/70 px-3 rounded-xl hover:bg-purple-500/80" onClick={() => markReadMutation.mutate()}>Mark read</button>}
      </p>
      {notifications.map((notification, i) =>
        <NotificationDisplay
          key={notification.id}
          notification={notification}
          className={i == notifications.length - 1 ? 'rounded-b-lg' : ''} 
        />)}
    </div>}
  </div>;
};
