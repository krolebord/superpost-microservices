import clsx from "clsx";
import { FC, ReactNode, useState } from "react";
import { Link, NavLink, Outlet } from "react-router-dom";
import { horizontalPadding } from "../common-styles";
import { MainSection } from "./MainSection";
import { ServiceStatus } from "./ServiceStatus";
import { useAuth } from "./auth/hooks";
import { Authorized } from "./auth/Authorized";
import { LoginDisplay, ProfileDisplay } from "./auth/AuthDisplay";
import { Notifications } from "./notifications/Notifications";

export const Header = () => {
  const { loading, user } = useAuth();

  return (
    <header className={clsx('grid grid-cols-[min-content,1fr,min-content] sm:grid-cols-[1fr,1fr,1fr] items-center border-b border-white/30', horizontalPadding)}>
      <Link to='/home' className="text-xl justify-self-start">
        <span className="hidden md:inline">SuperPost</span>
        <span className="inline md:hidden">SP</span>
      </Link>
      <nav className="inline-flex flex-row gap-2 justify-self-center">
        {!loading && user && <>
          <NavLink className={({isActive}) => isActive ? 'text-purple-400' : 'text-white'} to="/home">Home</NavLink>
          <NavLink className={({isActive}) => isActive ? 'text-purple-400' : 'text-white'} to="/new">New</NavLink>
        </>}
      </nav>
      <div className="justify-self-end flex flex-row gap-2 items-center">
        <Authorized authorized={<Notifications />} />
        <Authorized
          authorized={<ProfileDisplay />}
          unauthorized={<LoginDisplay />}
        />
      </div>
    </header>
  );
};

export const Footer = () => {
  const [showStatus, setShowStatus] = useState(false);

  return (
    <footer className={clsx('border-t border-white/20 flex gap-6 items-center justify-end', horizontalPadding)}>
      {showStatus && <span className="flex gap-4 flex-wrap">
        <ServiceStatus label="Auth: " healthCheckUrl="/api/auth/health" />
        <ServiceStatus label="Posts: " healthCheckUrl="/api/posts/health" />
        <ServiceStatus label="Users: " healthCheckUrl="/api/users/health" />
        <ServiceStatus label="Timeline: " healthCheckUrl="/api/timeline/health" />
        <ServiceStatus label="Notifications: " healthCheckUrl="/api/notifications/health" />
        <ServiceStatus label="Notifier: " healthCheckUrl="/api/notifier/health" />
      </span>}
      <button className="whitespace-nowrap" onClick={() => setShowStatus(x => !x)} >{showStatus ? 'Hide status' : 'Show status'}</button>
    </footer>
  );
};

export const RootLayout: FC<{ children?: ReactNode }> = ({ children }) => {
  return (<div className="h-full grid grid-rows-[2.5rem,minmax(min-content,1fr),min-content] items-stretch">
    <Header />
    <MainSection>
      {children ? children : <Outlet />}
    </MainSection>
    <Footer />
  </div>);
};
