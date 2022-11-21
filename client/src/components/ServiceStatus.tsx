import { useQuery } from '@tanstack/react-query';
import { FC, useEffect, useState } from 'react'

type Props = {
  label?: string;
  healthCheckUrl: string;
};

const getStatus = (url: string) => {
  return fetch(url)
    .then((response) => {
      if (!response.ok)
        throw new Error();

      return true;
    });
};

export const ServiceStatus: FC<Props> = (props) => {
  const { healthCheckUrl, label } = props;

  const { data: ok, isLoading, isError } = useQuery({
    queryKey: ['service-status', healthCheckUrl],
    queryFn: () => getStatus(healthCheckUrl),
    retry: 2,
    retryDelay(failureCount, error) {
      return 1000 * failureCount;
    },
  })

  return (<span>
    {label && <span>{label}</span>}
    {ok && <span>ok</span>}
    {isLoading && <span>loading...</span>}
    {isError && <span>down</span>}
  </span>)
}
