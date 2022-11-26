import clsx from "clsx";
import { FC } from "react";
import { Profile } from "../../api/users";
import { SubscribeButton } from "./SubscribeButton";
import { UserAvatar } from "./UserAvatar";

type ProfileCardProps = {
  profile: Profile;
  className?: string;
};

export const ProfileCard: FC<ProfileCardProps> = ({ profile, className }) => {
  return <div className={clsx('block p-3 bg-slate-700/50 rounded-xl', className)}>
    <div className="flex flex-row justify-between items-center pb-3 border-b border-b-slate-600">
      <div>
        <UserAvatar size="lg" userName={profile.name} />
        <span className="ml-3 text-2xl">{profile.name}</span>
      </div>
      <SubscribeButton userId={profile.id} />
    </div>
    <p className="flex justify-start gap-3 pt-3">
      <span>Subscribers: {profile.subscribersCount}</span>
      <span>Subscriptions: {profile.subscribedToCount}</span>
    </p>
  </div>;
};
