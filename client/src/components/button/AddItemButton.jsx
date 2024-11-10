import { Button } from '@nextui-org/button';
import { IconPlus } from '@tabler/icons-react';
import React from 'react';
import { useDisclosure } from '@nextui-org/modal';

import ModifyItemModal from '@/components/dialogs/ModifyItemModal';

export default function AddItemButton () {
  const {
    isOpen,
    onOpen,
    onClose,
    onOpenChange
  } = useDisclosure();

  return (
    <>
      <Button color={'primary'} startContent={<IconPlus />} onClick={() => {
        onOpen();
      }}>Add an item</Button>
      <ModifyItemModal isOpen={isOpen} onClose={onClose} onOpenChange={onOpenChange} />
    </>
  );
}
