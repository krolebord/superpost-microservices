import { QueryClient } from "@tanstack/react-query";

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: false,
      networkMode: 'always',
      staleTime: 2 * 1000
    }
  }
});
