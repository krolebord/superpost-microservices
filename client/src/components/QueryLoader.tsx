import { QueryOptions, useQuery } from "@tanstack/react-query";
import { ReactNode } from "react";

type QueryLoaderProps<TQuery extends QueryOptions & { queryFn: () => any }> = {
  query: TQuery,
  loaded: (data: Awaited<ReturnType<TQuery['queryFn']>>) => ReactNode,
}

export const QueryLoader = <TQuery extends QueryOptions & { queryFn: () => any }>({ query, loaded }: QueryLoaderProps<TQuery>) => {
  const { data, isLoading } = useQuery(query);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return <>{data && loaded(data as any)}</>;
};
