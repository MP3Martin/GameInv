import { v4 as uuidv4 } from 'uuid';

import { useGlobalStore } from '@/hooks/store/globalStore';
import { wsStatus } from '@/config/consts/enums';

/**
 * @param commandType {string}
 * @param data {string}
 */
export default async function sendMessage (commandType, data) {
  return new Promise((resolve, reject) => {
    const setTryResolveConfirm = useGlobalStore.getState().setTryResolveConfirm;
    const tryResolveConfirm = useGlobalStore.getState().tryResolveConfirm;
    // eslint-disable-next-line camelcase
    const get_sendMessage = () => useGlobalStore.getState()._sendMessage;
    const getWsStatus = () => useGlobalStore.getState().wsStatus;

    const messageUuid = uuidv4();

    // Checking
    if (tryResolveConfirm !== null) {
      myReject('Different message is already being sent');

      return;
    }
    if (get_sendMessage() === null) {
      myReject('_sendMessage is null');

      return;
    }
    if (getWsStatus() !== wsStatus.open) {
      myReject('Websocket is not open');

      return;
    }

    let message;

    try {
      message = `${commandType}|${messageUuid}|${btoa(data)}`;
    } catch (e) {
      myReject('Failed to encode message: ' + e);

      return;
    }

    setTryResolveConfirm(tryResolve);

    const timeoutTimer = setTimeout(() => {
      myResolve(false, 'Timeout');
    }, 5000);

    get_sendMessage()(message);

    // Beginning of functions

    function tryResolve (id, data) {
      if (id !== messageUuid) return;

      myResolve(data.success, data.message);
    }

    // Also handles cleanup
    function myResolve (success, message) {
      // Cleanup
      clearTimeout(timeoutTimer);
      setTryResolveConfirm(null);

      if (success) {
        resolve(message);
      } else {
        myReject(message);
      }
    }

    function myReject (message) {
      reject(new Error(message));
    }
  });
}
