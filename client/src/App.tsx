import { QueryClientProvider } from "@tanstack/react-query";
import { FC } from "react";
import { Posts } from "./components/Posts";
import { ServiceStatus } from "./components/ServiceStatus";
import { queryClient } from "./query-client";

export const Header = () => {
  return (
    <header className="flex items-center border-b border-white/30 px-4">
      <a className="" href="/">SuperPosts</a>
    </header>
  );
};

export const Footer = () => {
  return (
    <footer className="border-t border-white/20 flex gap-6 px-4">
      <ServiceStatus label="Posts service: " healthCheckUrl="/api/posts/health" />
      <ServiceStatus label="Users service: " healthCheckUrl="/api/users/health" />
    </footer>
  );
};

export const Root: FC<{ children?: React.ReactNode }> = ({ children }) => {
  return <QueryClientProvider client={queryClient}>
    {children}
  </QueryClientProvider>
}

export const App = () => {
  return (<Root>
    <Header />
    <main className="p-4">
      <Posts />
    </main>
    <Footer />
  </Root>)
}
