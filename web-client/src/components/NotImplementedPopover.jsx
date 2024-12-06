import { Popover, PopoverContent, PopoverTrigger } from '@nextui-org/popover';
import React from 'react';

export default function NotImplementedPopover ({ children }) {
  return (
    <Popover showArrow>
      <PopoverTrigger>
        {children}
      </PopoverTrigger>
      <PopoverContent>
        <span className={'m-1'}>Not implemented!</span>
      </PopoverContent>
    </Popover>
  );
}
