import { useMemo } from "react";
import { FC } from "react";
import clsx from 'clsx'

export type UserAvatarProps = {
  userName: string;
  size?: typeof sizes[number];
  className?: string;
}

const getAvatarLetters = (name: string) => {
  if (name.length <= 2) return name;

  let segments = name.split(' ');
  if (segments.length <= 1) segments = name.split('-');
  if (segments.length <= 1) segments = name.split('_');
  if (segments.length <= 1) segments = name.split('.');
  if (segments.length > 1)
    return `${segments[0][0].toUpperCase()}${segments[segments.length-1][0].toLocaleUpperCase()}`;

  const secondLetter = [...name].find((char, index) => index > 0 && char === char.toUpperCase());

  return name[0] + (secondLetter ?? name[1].toUpperCase());
}

const sizes = ['sm', 'md', 'lg'] as const;

const avatarSizes = {
  'sm': 'w-7 h-7',
  'md': 'w-10 h-10',
  'lg': 'w-14 h-14'
};

const fontSizes = {
  'sm': 'text-sm',
  'md': 'text-md',
  'lg': 'text-lg'
}

export const UserAvatar: FC<Props> = ({ userName, size = 'md', className }) => {
  const letters = useMemo(() => getAvatarLetters(userName), [userName]);

  return (<span className={clsx('inline-flex justify-center rounded-full items-center text-black bg-purple-400', avatarSizes[size], fontSizes[size], className)}>
    <span>{letters}</span>
  </span>)
}
