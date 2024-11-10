import { Modal, ModalBody, ModalContent, ModalFooter, ModalHeader } from '@nextui-org/modal';
import { Button } from '@nextui-org/button';
import React, { useEffect, useState } from 'react';

import ErrorInfoModal from '@/components/dialogs/ErrorInfoModal';
import sendMessage from '@/config/utils/ws/sendMessage';

/** @param isOpen
 @param onClose
 @param onOpenChange
 @param item {Item} */
export default function DeleteItemModal ({
  isOpen,
  onClose,
  onOpenChange,
  item
}) {
  const [errorMessage, setErrorMessage] = useState(null);
  const [locked, setLocked] = useState(false);

  /** @param item {Item} */
  function removeItem (item) {
    setLocked(true);
    sendMessage('remove_item', item.id).then(() => {
      setLocked(false);
      onClose();
    }).catch((reason) => {
      setLocked(false);
      setErrorMessage(reason.toString());
    });
  }

  useEffect(() => {
    setErrorMessage(null);
  }, [isOpen]);

  return (
    <Modal close backdrop={'opaque'} hideCloseButton={locked} isOpen={isOpen} scrollBehavior={'inside'}
           onOpenChange={onOpenChange}>
      <ModalContent>
        <ModalHeader>Confirmation</ModalHeader>
        <ModalBody>
          <p>Are you sure you want to remove {`"${item.name}"`}?</p>
        </ModalBody>
        <ModalFooter>
          <Button color="default" isDisabled={locked} variant="light" onPress={onClose}>
            Cancel
          </Button>
          <Button color="danger" isDisabled={locked} isLoading={locked} onPress={() => {
            removeItem(item);
          }}>
            Delete
          </Button>
          <ErrorInfoModal message={errorMessage} setErrorMessage={setErrorMessage} />
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
