import React, { useEffect, useState } from 'react';
import { Chip } from '@nextui-org/chip';
import { Spinner } from '@nextui-org/spinner';

import { useGlobalStore } from '@/hooks/store/globalStore';
import { wsStatus as wsStatusEnum } from '@/config/consts/enums';

/** @type {Record<string, [string, string]>} */
const statusDataMap = {
  [wsStatusEnum.error]: ['danger', 'Invalid WS URI'],
  [wsStatusEnum.off]: ['default', 'Off'],
  [wsStatusEnum.closed]: ['default', 'Disconnected'],
  [wsStatusEnum.connecting]: ['warning', 'Connecting'],
  [wsStatusEnum.open]: ['success', 'Connected']
};

export default function WsStatus ({ ...props }) {
  const wsStatus = useGlobalStore((state) => state.wsStatus);

  useEffect(() => {
    setStatusData(statusDataMap[wsStatus]);
  }, [wsStatus]);

  const [statusData, setStatusData] = useState(
    /** @type {[any, any]} */
    statusDataMap[wsStatusEnum.closed]
  );

  return (
    <span {...props}>
      <Chip
        color={statusData[0]}
        endContent={wsStatus === wsStatusEnum.connecting
          ? <Spinner color={statusData[0]} size={'sm'} />
          : null}
        variant="bordered"
      >
        {statusData[1]}
      </Chip>
    </span>
  );
}
