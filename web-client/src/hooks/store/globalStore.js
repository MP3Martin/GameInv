import { create } from 'zustand';
import { immer } from 'zustand/middleware/immer';
import { persist } from 'zustand/middleware';
import merge from 'lodash.merge';

import { wsStatus } from '@/config/consts/enums';

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export const useGlobalStore = create(persist(immer((set, get) => ({
  persist: {
    settings: {
      wsAddress: 'ws://localhost:9081',
      wsPassword: 'changeme',
      wsEnabled: true
    },
    setSettings: (value) => set((state) => { state.persist.settings = value; })
  },
  wsStatus: wsStatus.closed,
  setWsStatus: (value) => set((state) => { state.wsStatus = value; }),

  /** @type {global.Item[]} */
  items: [],
  setItems: (value) => set((state) => { state.items = value; }),

  tryResolveConfirm: null,
  setTryResolveConfirm: (value) => set((state) => { state.tryResolveConfirm = value; }),

  _sendMessage: null,
  set_sendMessage: (value) => set((state) => { state._sendMessage = value; })
})),
{
  name: 'GameInv-useGlobalStore-persist',
  partialize: (state) => ({ persist: state.persist }),
  merge: /* Thanks to amiellion @ https://github.com/pmndrs/zustand/issues/457#issuecomment-1929123698 */
      (persistedState, currentState) => {
        return merge({}, currentState, persistedState);
      }
}));
