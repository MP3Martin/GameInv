import React from 'react';
import { useDisclosure } from '@nextui-org/modal';
import { Button } from '@nextui-org/button';
import { IconSettings } from '@tabler/icons-react';

import SettingsModal from '@/components/dialogs/SettingsModal';
import MyTooltip from '@/components/MyTooltip';

export default function SettingsButton ({ tooltipPlacement = 'bottom' }) {
  const {
    isOpen,
    onOpen,
    onClose,
    onOpenChange
  } = useDisclosure();

  return (
    <>
      <MyTooltip content="Settings" placement={tooltipPlacement}>
        <Button isIconOnly size={'md'} variant={'light'} onPress={onOpen}>
          <IconSettings className="text-default-500" size={28} />
        </Button>
      </MyTooltip>
      <SettingsModal isOpen={isOpen} onClose={onClose} onOpenChange={onOpenChange} />
    </>
  );
}
