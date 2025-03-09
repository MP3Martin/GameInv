import { Modal, ModalBody, ModalContent, ModalFooter, ModalHeader } from '@nextui-org/modal';
import { Button } from '@nextui-org/button';
import { useImmer } from 'use-immer';
import { Input } from '@nextui-org/input';
import { useEffect } from 'react';
import { Switch } from '@nextui-org/switch';

import { useGlobalStore } from '@/hooks/store/globalStore';
import PasswordInput from '@/components/PasswordInput';

export default function SettingsModal ({
  isOpen,
  onOpenChange
}) {
  const settings = useGlobalStore((state) => state.persist.settings);
  const setSettings = useGlobalStore((state) => state.persist.setSettings);

  const [settingsData, setSettingsData] = useImmer(settings);

  useEffect(() => {
    if (isOpen) {
      setSettingsData(settings);
    }
  }, [isOpen]);

  function saveSettings () {
    setSettings(settingsData);
  }

  /**
   * @param event {React.ChangeEvent<HTMLInputElement>}
   * @param parseValue {(value: string) => any}
   */
  const handleOnChange = (event, parseValue = null) => {
    let {
      name,
      value
    } = event.target;

    if (parseValue !== null) {
      value = parseValue(value);
    }

    setSettingsData((draft) => {
      draft[name] = value;
    });
  };

  return (
    <Modal backdrop={'opaque'} isOpen={isOpen} placement={'center'}
           scrollBehavior={'inside'} onOpenChange={onOpenChange}>
      <ModalContent>
        {(onClose) => (
          <>
            <ModalHeader className="flex flex-col gap-1">{'Settings'}</ModalHeader>
            <ModalBody>
              <Input label={'Websocket Address'} name={'wsAddress'} value={settingsData.wsAddress}
                     onChange={handleOnChange} />
              <PasswordInput label={'Websocket Password'} name={'wsPassword'} value={settingsData.wsPassword}
                             onChange={handleOnChange} />
              <Switch isSelected={settingsData.wsEnabled} name={'wsEnabled'} onValueChange={(val) => {
                // @ts-expect-error
                handleOnChange({
                  target: {
                    name: 'wsEnabled',
                    value: val
                  }
                });
              }}>WebSocket
                connection</Switch>
            </ModalBody>
            <ModalFooter>
              <Button typ color="danger" variant="light" onPress={onClose}>
                Cancel
              </Button>
              <Button color="primary" onPress={() => {
                saveSettings();
                onClose();
              }}>
                Save
              </Button>
            </ModalFooter>
          </>
        )}
      </ModalContent>
    </Modal>
  );
}
