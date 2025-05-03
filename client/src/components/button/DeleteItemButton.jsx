import React from 'react';
import { useDisclosure } from '@nextui-org/modal';
import { Tooltip } from '@nextui-org/tooltip';
import { Button } from '@nextui-org/button';
import { IconTrash } from '@tabler/icons-react';

import DeleteItemModal from '@/components/dialogs/DeleteItemModal';

/** @param item {Item} */
export default function DeleteItemButton ({ item }) {
  const {
    isOpen,
    onOpen,
    onClose,
    onOpenChange
  } = useDisclosure();

  return (
    <>
      <Tooltip showArrow content="Delete item" placement="bottom">
        <Button isIconOnly color="danger" onPress={onOpen}><IconTrash /></Button>
      </Tooltip>
      <DeleteItemModal isOpen={isOpen} item={item} onClose={onClose} onOpenChange={onOpenChange} />
    </>
  );
}
