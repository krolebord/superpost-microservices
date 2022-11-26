import clsx from "clsx";
import { Outlet } from "react-router-dom";
import { cardHorizontalMargin } from "../../common-styles";

export const AuthLayout = () => {
  return (
    <div className={clsx('h-full grid items-center sm:px-12 md:px-14', cardHorizontalMargin)}>
      <Outlet />
    </div>);
}
