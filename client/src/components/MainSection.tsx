import clsx from "clsx";
import { FC, ReactElement, ReactNode } from "react";
import { horizontalPadding } from "../common-styles";

export const MainSection: FC<{ children: ReactNode }> = ({ children }) => {
  return (<main className={clsx('py-3', horizontalPadding)}>
    {children}
  </main>)
};
