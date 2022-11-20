import { ServiceStatus } from "./ServiceStatus";

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

export const App = () => {
  return (<>
    <Header />
    <main className="p-4">
      <h1>React App</h1>
    </main>
    <Footer />
  </>)
}
