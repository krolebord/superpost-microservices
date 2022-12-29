import { useQuery } from "@tanstack/react-query"
import { Notification, notificationsQuery, notificationsSSEUrl } from "../../api/notifications"
import { useCallback, useRef, useSyncExternalStore } from "react";

export const useNotifications = () => {
  const notificationsRef = useRef<Notification[]>([]);

  useQuery({
    ...notificationsQuery(),
    placeholderData: [],
    refetchOnWindowFocus: false,
    onSuccess(notifications) {
      notificationsRef.current = notifications;
    },
  });

  const getNotifications = useCallback(() => notificationsRef.current, [notificationsRef]);

  const subscribe = useCallback((onStoreChange: () => void) => {
    const source = new EventSource(notificationsSSEUrl, { withCredentials: true });

    source.onmessage = (event) => {
      const newNotification = JSON.parse(event.data) as Notification;
      notificationsRef.current = [newNotification, ...notificationsRef.current];
      onStoreChange();
    };

    return () => {
      source.close();
    }
  }, [notificationsRef]);

  return useSyncExternalStore(subscribe, getNotifications)
}
