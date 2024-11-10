import { Popover, PopoverContent, PopoverTrigger } from '@nextui-org/popover';
import React from 'react';

import MyTooltip from '@/components/MyTooltip';

/** @param trigger
 * @param title {String?}
 * @param content {JSX.Element}
 * @param trigger {JSX.Element}
 * @param isOpen {Boolean}
 * @param onOpenChange {(isOpen: boolean) => void}
 * @param tooltipPlacement
 * @param popoverPlacement {(import('@nextui-org/aria-utils').OverlayPlacement)?}
 */
export default function MyPopover ({
  trigger,
  title = null,
  content,
  isOpen,
  onOpenChange,
  popoverPlacement = null,
  tooltipPlacement = 'bottom'
}) {
  return (
    <Popover showArrow backdrop={'opaque'} isOpen={isOpen} placement={popoverPlacement ?? 'bottom'}
             onOpenChange={onOpenChange}>
      <MyTooltip content={title} hidden={isOpen} placement={tooltipPlacement}>
        <span>
          <PopoverTrigger>
            {trigger}
          </PopoverTrigger>
        </span>
      </MyTooltip>
      <PopoverContent className={'popover-fix-z-index'}>
        <div className="px-1 py-2 max-w-64 sm:max-w-md">
          {title && <div className="text-large font-bold">{title}</div>}
          <div className="text-medium">
            <div className="px-1 pt-2 max-w-64 sm:max-w-md">
              {content}
            </div>
          </div>
        </div>
      </PopoverContent>
    </Popover>
  );
}
