import { FC } from "react";
import { Link } from "react-router-dom";
import { addPrefix } from "../../type-helpers";
import { UserAvatar, UserAvatarProps } from "./UserAvatar";

type UserDisplayProps = {
  userId: string;
  userName: string;
} & addPrefix<Omit<UserAvatarProps, 'userName'>, 'avatar'>;

export const UserDisplay: FC<UserDisplayProps> = ({ userName, userId, avatarSize, avatarClassName }) => {
  return <Link to={`/profile/${userId}`} className="whitespace-nowrap" >
    <UserAvatar size={avatarSize} className={avatarClassName} userName={userName} />
    <span className="ml-1 peer-hover:invisible">{userName}</span>
  </Link>
};
