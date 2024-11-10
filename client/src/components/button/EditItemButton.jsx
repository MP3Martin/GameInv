import { Button } from '@nextui-org/button';
import { IconEdit } from '@tabler/icons-react';
import React from 'react';
import { useDisclosure } from '@nextui-org/modal';

import ModifyItemModal from '@/components/dialogs/ModifyItemModal';
import MyTooltip from '@/components/MyTooltip';

export default function EditItemButton ({ item }) {
  const {
    isOpen,
    onOpen,
    onClose,
    onOpenChange
  } = useDisclosure();

  return (
    <>
      <MyTooltip content="Edit item">
        <Button isIconOnly variant={'light'} onClick={onOpen}>
          <IconEdit className="text-default-500" size={28} />
        </Button>
      </MyTooltip>
      <ModifyItemModal editItem={item} isOpen={isOpen} onClose={onClose} onOpenChange={onOpenChange} />
    </>
  );
}
