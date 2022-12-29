import { FC, useState } from "react";
import { haltQuery, toggleHalt } from "../../api/posts";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getNewTimeline } from "../../api/timeline";

export const Halt: FC = () => {
  const client = useQueryClient();

  const { data: isHalted, isInitialLoading } = useQuery({
    ...haltQuery,
    refetchOnMount: false,
    refetchOnWindowFocus: false,
  });
  const { mutate: togleHalt, isLoading: toggling } = useMutation({
    mutationFn: toggleHalt,
    onMutate: async () => {
      await client.cancelQueries(haltQuery.queryKey);
      const wasHalted = client.getQueryData<boolean>(haltQuery.queryKey);

      if (wasHalted !== undefined) {
        client.setQueryData<boolean>(haltQuery.queryKey, old => !old);
      }

      return { wasHalted };
    },
    onError: (_, __, context) => {
      client.setQueryData(haltQuery.queryKey, context?.wasHalted)
    },
    onSettled: () => {
      client.invalidateQueries({ queryKey: haltQuery.queryKey })
    },
  });

  if (isInitialLoading) return <div>Loading...</div>;

  return <>
    <div>
      <p>Artificial latency is {isHalted ? 'enabled' : 'disabled'}</p>
      <button
        className="border border-gray-300 rounded-md px-2 py-1"
        disabled={isInitialLoading || toggling}
        onClick={() => togleHalt()}
      >
          { isHalted ? 'Disable' : 'Enable' }
      </button>
    </div>
  </>
}

export const RequestTimeTester: FC = () => {
  const [avgResponse, setAvgResponse] = useState<number | null>(null);

  const testAvgResponse = async () => {
    const getNewPageResponseTime = async () => {
      const start = performance.now();
      await getNewTimeline();
      return performance.now() - start;
    }

    const responseTime = await getNewPageResponseTime();

    setAvgResponse(responseTime);
  };


  return <>
    <div>
      <p>Average response time: {avgResponse ? `${Math.round(avgResponse)}ms` : 'untested'}</p>
      <button
        className="border border-gray-300 rounded-md px-2 py-1"
        onClick={() => testAvgResponse()}
      >
        Test
      </button>
    </div>
  </>
}

export const HaltPage: FC = () => {
  return <div>
    <Halt />
    <RequestTimeTester />
  </div>
}
