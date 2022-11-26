import { RootLayout } from "./RootLayout";

export const NotFound = () => {
  return (<RootLayout>
    <div className="flex flex-col items-center justify-center h-full">
      <h1 className="text-4xl">404</h1>
      <p className="text-xl">Not Found</p>
    </div>
  </RootLayout>)
};
