import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { FC } from "react";
import { Link } from "react-router-dom";
import { isSubscribedQuery, profileQuery, subscribe, unsubscribe } from "../../api/users";
import { SignInLink } from "../auth/AuthDisplay";
import { useAuth } from "../auth/hooks";

type SubscribeButtonProps = {
  userId: string;
}

export const SubscribeButton: FC<SubscribeButtonProps> = ({ userId }) => {
  const { loading: loadingUser , user } = useAuth();

  const { data: isSubscribed, isLoading } = useQuery({
    ...isSubscribedQuery(userId),
    enabled: !loadingUser && !!user,
  });

  const queryClient = useQueryClient();
  const subscribeMutation = useMutation({
    mutationFn: subscribe,
    onSuccess: () => {
      queryClient.setQueryData(isSubscribedQuery(userId).queryKey, true);
      queryClient.invalidateQueries(profileQuery(userId).queryKey);
    }
  });
  const ubsubscribeMutation = useMutation({
    mutationFn: unsubscribe,
    onSuccess: () => {
      queryClient.setQueryData(isSubscribedQuery(userId).queryKey, false);
      queryClient.invalidateQueries(profileQuery(userId).queryKey);
    }
  });

  if (!loadingUser) {
    if (!user) return <SignInLink className="bg-purple-400 rounded-full px-4 py-2 text-black" />
    if (user.id === userId) return <span className="bg-purple-400 rounded-full px-4 py-2 text-black">Your Profile</span>;
  }

  const loading = isLoading || loadingUser || subscribeMutation.isLoading || ubsubscribeMutation.isLoading;

  const onButtonClick = () => {
    if (isSubscribed) {
      ubsubscribeMutation.mutate(userId);
    } else {
      subscribeMutation.mutate(userId);
    }
  };

  return <button className="bg-purple-400 rounded-full px-4 py-2 text-black" disabled={loading} onClick={onButtonClick}>
    {loading ? 'Loading...' :
      isSubscribed ? 'Unsububscribe' : 'Subscribe'}
    
  </button>
}
