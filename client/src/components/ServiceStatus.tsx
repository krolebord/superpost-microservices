import { useQuery } from '@tanstack/react-query';
import { FC } from 'react'

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
    retry: false
  })

  return (<span className="inline-flex gap-1">
    {label && <span>{label}</span>}
    {ok && <span>🟢</span>}
    {isLoading && <span>🟡</span>}
    {isError && <span>🔴</span>}
  </span>)
}
