import { v4 as uuidv4 } from 'uuid';
import { Base64 } from 'js-base64';

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

    // Checking and handling errors

    if (tryResolveConfirm !== null) return myReject('Different message is already being sent');
    if (get_sendMessage() === null) return myReject('_sendMessage is null');
    if (getWsStatus() !== wsStatus.open) return myReject('Websocket is not open');

    let message;

    try {
      message = `${commandType}|${messageUuid}|${Base64.encode(data.toString())}`;
    } catch (e) {
      return myReject('Failed to encode message: ' + e);
    }

    // Send the message

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

    /**
     * Also handles cleanup
     */
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

    /**
     * Reject without any cleanup
     */
    function myReject (message) {
      reject(new Error(message));
    }
  });
}
