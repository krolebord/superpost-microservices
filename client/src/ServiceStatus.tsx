import { FC, useEffect, useState } from 'react'

type Props = {
  label?: string;
  healthCheckUrl: string;
};

export const ServiceStatus: FC<Props> = (props) => {
  const { healthCheckUrl, label } = props;

  const [status, setStatus] = useState<'loading' | 'ok' | 'error'>('loading');

  useEffect(() => {
    const abortController = new AbortController();
    fetch(healthCheckUrl, { signal: abortController.signal })
      .then((response) => {
        return response.text();
      })
      .then((data) => {
        setStatus(data === 'ok' ? 'ok' : 'error');
      })
      .catch(() => {
        setStatus('error');
      });

    return () => {
      abortController.abort();
    }
  }, [healthCheckUrl]);

  return (<span>
    {label && <span>{label}</span>}
    <span>{status}</span>
  </span>)
}
