import { camelizeKeys } from 'humps';

import { useGlobalStore } from '@/hooks/store/globalStore';

function handleItems (items) {
  try {
    useGlobalStore.getState().setItems(items);
  } catch (e) {
    // eslint-disable-next-line no-console
    console.error(e);
  }
}

/**
 * @param {string} message
 */
export const handleMessage = (message) => {
  let commandType;
  let messageUuid;
  let messageData;

  try {
    const messageParts = message.split('|');

    if (messageParts.length !== 3) return;
    [commandType, messageUuid, messageData] = messageParts;
    messageData = atob(messageData);
    messageData = JSON.parse(messageData);
    messageData = camelizeKeys(messageData);
  } catch { return; }
  switch (commandType) {
    case 'items': {
      handleItems(messageData);
      break;
    }
    case 'confirm': {
      useGlobalStore.getState().tryResolveConfirm?.(messageUuid, messageData);
      break;
    }
  }
};
