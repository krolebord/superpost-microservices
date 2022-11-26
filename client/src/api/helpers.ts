import { QueryClient, QueryOptions } from "@tanstack/react-query";

export type InferLoaderData<T extends (...args: any) => any> = Exclude<Awaited<ReturnType<T>>, Response>;

export const typedFetch = <T>(url: string, options?: RequestInit): Promise<T> => {
  return fetch(url, options).then((res) => res.ok ? res.json() : Promise.reject(res)) as Promise<T>;
} 
