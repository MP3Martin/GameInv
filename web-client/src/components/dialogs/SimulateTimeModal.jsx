import { Modal, ModalBody, ModalContent, ModalFooter, ModalHeader } from '@nextui-org/modal';
import { Button } from '@nextui-org/button';
import { Input } from '@nextui-org/input';
import { useEffect, useState } from 'react';

import sendMessage from '@/config/utils/ws/sendMessage';
import ErrorInfoModal from '@/components/dialogs/ErrorInfoModal';

const defaultTime = 20;

/** @param isOpen
 * @param onOpenChange
 * @param onClose
 * @param editItem {Item} */
export default function SimulateTimeModal ({
  isOpen,
  onOpenChange,
  onClose
}) {
  const [time, setTime] = useState(defaultTime);
  const [locked, setLocked] = useState(false);

  const [errorMessage, setErrorMessage] = useState(null);

  useEffect(() => {
    setTime(defaultTime);
    setErrorMessage(null);
  }, [isOpen]);

  function tickTime (time) {
    setLocked(true);
    sendMessage('tick_time', time).then(() => {
      setLocked(false);
      onClose();
    }).catch((reason) => {
      setLocked(false);
      setErrorMessage(reason.toString());
    });
  }

  /**
   * @param event {React.ChangeEvent<HTMLInputElement>}
   */
  const handleOnChange = (event) => {
    const {
      value
    } = event.target;

    setTime(value);
  };

  function onOpenChangeWrapper (...args) {
    if (!locked) onOpenChange(...args);
  }

  return (
    <Modal backdrop={'opaque'} hideCloseButton={locked} isOpen={isOpen}
           placement={'center'} scrollBehavior={'inside'} onOpenChange={onOpenChangeWrapper}>
      <ModalContent>
        {(onClose) => {
          return (
            <>
              <ModalHeader
                className="flex flex-col gap-1">Simulate time</ModalHeader>
              <ModalBody>
                <Input isDisabled={locked} label={'Tick amount'} name={'time'}
                       value={time} onChange={handleOnChange} />
              </ModalBody>
              <ModalFooter>
                <Button color="danger" isDisabled={locked} variant="light" onPress={onClose}>
                  Cancel
                </Button>
                <Button color="primary" isDisabled={locked} isLoading={locked} onPress={() => {
                  tickTime(time);
                }}>
                  Confirm
                </Button>
              </ModalFooter>
              <ErrorInfoModal message={errorMessage} setErrorMessage={setErrorMessage} />
            </>
          );
        }}
      </ModalContent>
    </Modal>
  );
}
