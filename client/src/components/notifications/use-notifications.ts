import { useQuery } from "@tanstack/react-query"
import { Notification, notificationsQuery, notificationsSSEUrl } from "../../api/notifications"
import { useCallback, useRef, useSyncExternalStore } from "react";

export const useNotifications = () => {
  const notificationsRef = useRef<Notification[]>([]);

  useQuery({
    ...notificationsQuery(),
    placeholderData: [],
    onSuccess(notifications) {
      notificationsRef.current = notifications;
    },
  });

  const getNotifications = useCallback(() => notificationsRef.current, [notificationsRef]);

  const subscribe = useCallback((onStoreChange: () => void) => {
    const source = new EventSource(notificationsSSEUrl, { withCredentials: true });

    source.onmessage = (event) => {
      console.log(event);
      onStoreChange();
      // const notifications = JSON.parse(event.data);
      // notificationsRef.current = notifications;
    };
    console.log("subscribed to notifications");
    return () => {
      source.close();
    }
  }, [notificationsRef]);

  return useSyncExternalStore(subscribe, getNotifications)
}
