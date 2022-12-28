import { FC, ReactNode } from "react";
import { useAuth } from "./hooks";

export type AuthorizedProps ={
  loading?: ReactNode;
  authorized?: ReactNode;
  unauthorized?: ReactNode;
}

export const Authorized: FC<AuthorizedProps> = (props) => {
  const { loading, authorized, unauthorized } = props;
  const { user, loading: authLoading } = useAuth();

  if (authLoading) {
    return <>{loading}</> ?? <></>;
  }

  return (<>
    {user ? authorized ?? <></> : unauthorized ?? <></> }
  </>);
}
