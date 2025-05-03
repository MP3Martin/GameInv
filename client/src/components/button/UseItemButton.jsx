import React, { useState } from 'react';
import { Button } from '@nextui-org/button';
import { IconPick } from '@tabler/icons-react';

import sendMessage from '@/config/utils/ws/sendMessage';
import MyTooltip from '@/components/MyTooltip';

/** @param item {Item} */
export default function UseItemButton ({ item }) {
  const [locked, setLocked] = useState(false);

  /** @param item {Item} */
  function _useItem (item) {
    setLocked(true);
    sendMessage('use_item', item.id).then(() => {
      setLocked(false);
    }).catch(() => {
      setLocked(false);
    });
  }

  return (
    <>
      <MyTooltip content="Use item">
        <Button isIconOnly isDisabled={locked || (item.damagePerUse == null)} isLoading={locked} variant={'light'}
                onPress={() => {
                  _useItem(item);
                }}><IconPick className="text-default-500" size={28} /></Button>
      </MyTooltip>
    </>
  );
}
