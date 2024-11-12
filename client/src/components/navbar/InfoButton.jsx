import React from 'react';
import { useDisclosure } from '@nextui-org/modal';
import { IconInfoCircle } from '@tabler/icons-react';
import { Button } from '@nextui-org/button';
import { Link } from '@nextui-org/link';

import MyPopover from '@/components/navbar/MyPopover';

export default function InfoButton ({ tooltipPlacement = 'bottom' }) {
  const {
    isOpen,
    onOpen,
    onClose,
    onOpenChange
  } = useDisclosure();

  return (
    <MyPopover content={
      <div className="info-button-popover-scroll">
        This website allows you to connect to GameInv server using a Next.js NextUI web interface.
        <br /><br />
        Credits:
        <div className={'pl-4'}>
          <ul className={'list-disc list-outside break-words'}>
            {
              [
                ['[OTHER]', 'https://github.com/MP3Martin/GameInv/blob/main/client/package.json'],
                ['Soumya Ranjan Padhy - useScreenSize', 'https://dev.to/soumyarian/usescreensize-a-custom-react-hook-for-dynamic-screen-size-detection-5e59'],
                ['@tabler/icons-react'],
                ['@nextui-org/react'],
                ['next'],
                ['react'],
                ['zustand'],
                ['immer'],
                ['framer-motion'],
                ['javascript-color-gradient'],
                ['styled-components'],
                ['js-base64']
              ].map((item) => (
                <li key={item[0]}>
                  <Link
                    isExternal className={'text-medium'}
                    href={item[1] ?? 'https://www.npmjs.com/package/' + item[0]}>
                    {item[0]}
                  </Link>
                </li>
              ))
            }
          </ul>
        </div>
      </div>
    } isOpen={isOpen} popoverPlacement={'bottom-end'} title="Info" tooltipPlacement={tooltipPlacement} trigger={
      <Button isIconOnly size={'md'} variant={'light'} onClick={() => isOpen ? onClose : onOpen}>
        <IconInfoCircle className="text-default-500" size={28} />
      </Button>
    } onOpenChange={onOpenChange} />
  );
}
