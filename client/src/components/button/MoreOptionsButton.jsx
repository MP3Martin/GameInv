import { Button } from '@nextui-org/button';
import { IconDotsVertical } from '@tabler/icons-react';
import React from 'react';
import { useDisclosure } from '@nextui-org/modal';

import MyPopover from '@/components/navbar/MyPopover';
import DeleteItemButton from '@/components/button/DeleteItemButton';
import MyTooltip from '@/components/MyTooltip';
import EditItemButton from '@/components/button/EditItemButton';
import { useScreenSize } from '@/hooks/useScreenSize';

export default function MoreOptionsButton ({ item }) {
  const {
    isOpen,
    onOpenChange
  } = useDisclosure();

  const screenSize = useScreenSize();

  return (
    <>
      <MyPopover content={
        <div className={'flex flex-row justify-center'}>
          <div className={'flex flex-row gap-2'}>
            <EditItemButton item={item} />
            <MyTooltip content="Delete item">
              <DeleteItemButton item={item} />
            </MyTooltip>
          </div>
        </div>
      } isOpen={isOpen} popoverPlacement={((['xs', 'xxs'].includes(screenSize)) ? 'bottom-start' : null)}
                 title={'More options'}
                 trigger={
                   <Button isIconOnly size={'md'} variant={'light'}>
                     <IconDotsVertical className="text-default-500" size={28} />
                   </Button>
                 } onOpenChange={onOpenChange} />
    </>
  );
}
