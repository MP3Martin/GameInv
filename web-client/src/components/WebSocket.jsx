import React, { useEffect, useState } from 'react';
import useWebSocket, { ReadyState } from 'react-use-websocket';
import { emitCustomEvent } from 'react-custom-events';

import { useGlobalStore } from '@/hooks/store/globalStore';
import { isValidWebSocketURI } from '@/config/utils';
import { events, wsStatus } from '@/config/consts/enums';
import { handleMessage } from '@/config/utils/ws/wsHandler';

export const WebSocket = () => {
  const wsAddress = useGlobalStore((state) => state.persist.settings.wsAddress);
  const setWsStatus = useGlobalStore((state) => state.setWsStatus);

  const wsPassword = useGlobalStore((state) => state.persist.settings.wsPassword);

  const wsEnabled = useGlobalStore((state) => state.persist.settings.wsEnabled);
  const setItems = useGlobalStore((state) => state.setItems);

  useEffect(() => {
    if (isValidWebSocketURI(wsAddress)) {
      setWsUri(wsAddress);
    } else {
      setWsUri(null);
    }
  }, [wsAddress]);

  const [wsUri, setWsUri] = useState(null);

  const {
    sendMessage,
    readyState
  } = useWebSocket(wsUri, {
    shouldReconnect: () => true,
    onMessage: (message) => {
      handleMessage(message.data);
    },
    onOpen: () => {
      // Auth
      sendMessage(wsPassword);
    }
  }, wsEnabled);

  const connectionStatus = {
    [ReadyState.CONNECTING]: 'Connecting',
    [ReadyState.OPEN]: 'Open',
    [ReadyState.CLOSING]: 'Closing',
    [ReadyState.CLOSED]: 'Closed',
    [ReadyState.UNINSTANTIATED]: 'Uninstantiated'
  }[readyState];

  // eslint-disable-next-line camelcase
  const set_sendMessage = useGlobalStore((state) => state.set_sendMessage);

  useEffect(() => {
    if (readyState === ReadyState.CLOSED || readyState === ReadyState.UNINSTANTIATED) {
      emitCustomEvent(events.ws.connection.disconnect);
      set_sendMessage(null);
      setItems([]);
    }

    set_sendMessage(sendMessage);

    const determineWsStatus = () => {
      if (!wsEnabled) {
        return wsStatus.off;
      }

      if (wsUri === null) {
        return wsStatus.error;
      }

      switch (connectionStatus) {
        case 'Open':
          return wsStatus.open;
        case 'Connecting':
          return wsStatus.connecting;
        case 'Closed':
        case 'Closing':
          return wsStatus.closed;
        case 'Uninstantiated':
          return wsStatus.off;
        default:
          return null;
      }
    };

    const newWsStatus = determineWsStatus();

    if (newWsStatus !== null) {
      setWsStatus(newWsStatus);
    }
  }, [connectionStatus, wsUri, wsEnabled]);

  return (
    <></>
  );
};
