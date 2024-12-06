import { Tooltip } from '@nextui-org/tooltip';
import React from 'react';

export default function MyTooltip ({
  children,
  content,
  placement = 'bottom',
  ...props
}) {
  return (
    <Tooltip showArrow closeDelay={10} content={content} placement={placement} {...props}>
      {children}
    </Tooltip>
  );
}
