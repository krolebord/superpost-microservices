import clsx from "clsx";
import { Outlet, useNavigate } from "react-router-dom";
import { cardHorizontalMargin } from "../../common-styles";
import { useAuth } from "./hooks";
import { useEffect } from "react";

export const AuthLayout = () => {
  const navigate = useNavigate();
  const { loading, user } = useAuth();

  useEffect(() => {
    if (!loading && user) {
      navigate('/home', { replace: true });
    }
  }, [navigate, loading, user]);

  return (
    <div className={clsx('h-full grid items-center sm:px-12 md:px-14', cardHorizontalMargin)}>
      <Outlet />
    </div>);
}
