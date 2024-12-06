import { Modal, ModalBody, ModalContent, ModalFooter, ModalHeader, useDisclosure } from '@nextui-org/modal';
import { Button } from '@nextui-org/button';
import { useEffect } from 'react';

export default function ErrorInfoModal ({
  message,
  setErrorMessage
}) {
  const {
    isOpen,
    onClose,
    onOpen,
    onOpenChange
  } = useDisclosure();

  useEffect(() => {
    if (message != null) {
      onOpen();
    }
  }, [message]);

  useEffect(() => {
    if (!isOpen) {
      setErrorMessage(null);
    }
  }, [isOpen]);

  return (
    <Modal backdrop={'opaque'} isOpen={isOpen} scrollBehavior={'inside'} onOpenChange={onOpenChange}>
      <ModalContent>
        <ModalHeader className={'text-danger'}>Error</ModalHeader>
        <ModalBody>
          <p>{message}</p>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onPress={onClose}>
            OK
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
