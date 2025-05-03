import { Button } from '@nextui-org/button';
import { IconClock } from '@tabler/icons-react';
import React from 'react';
import { useDisclosure } from '@nextui-org/modal';

import SimulateTimeModal from '@/components/dialogs/SimulateTimeModal';

export default function SimulateTimeButton () {
  const {
    isOpen,
    onOpen,
    onClose,
    onOpenChange
  } = useDisclosure();

  return (
    <>
      <Button color={'secondary'} startContent={<IconClock />} onPress={() => {
        onOpen();
      }}>Simulate time</Button>
      <SimulateTimeModal isOpen={isOpen} onClose={onClose} onOpenChange={onOpenChange} />
    </>
  );
}
