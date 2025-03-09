import { Modal, ModalBody, ModalContent, ModalFooter, ModalHeader } from '@nextui-org/modal';
import { Button } from '@nextui-org/button';
import { useImmer } from 'use-immer';
import { Input } from '@nextui-org/input';
import { v4 as uuidv4 } from 'uuid';
import { useEffect, useState } from 'react';

import sendMessage from '@/config/utils/ws/sendMessage';
import ErrorInfoModal from '@/components/dialogs/ErrorInfoModal';
import { siteConfig } from '@/config/consts/site';

const numberInputInfo = `(number, not required, 0-${siteConfig.ushortMax})`;

/** @param isOpen
 * @param onOpenChange
 * @param onClose
 * @param editItem {Item} */
export default function ModifyItemModal ({
  isOpen,
  onOpenChange,
  onClose,
  editItem = null
}) {
  const defaultValue = editItem ?? {};
  const [item, setItem] = useImmer(defaultValue);
  const [locked, setLocked] = useState(false);

  const [errorMessage, setErrorMessage] = useState(null);

  useEffect(() => {
    setItem(defaultValue);
    setErrorMessage(null);
  }, [isOpen]);

  function modifyItem (itemData) {
    const item = structuredClone(itemData);

    item.id ??= uuidv4();
    setLocked(true);
    sendMessage('modify_item', JSON.stringify(item)).then(() => {
      setLocked(false);
      onClose();
    }).catch((reason) => {
      setLocked(false);
      setErrorMessage(reason.toString());
    });
  }

  /**
   * @param event {React.ChangeEvent<HTMLInputElement>}
   * @param parseValue {(value: string) => any}
   */
  const handleOnChange = (event, parseValue = null) => {
    let {
      name,
      value
    } = event.target;

    if (parseValue !== null) {
      value = parseValue(value);
    }

    setItem((draft) => {
      draft[name] = value;
    });
  };

  function onOpenChangeWrapper (...args) {
    if (!locked) onOpenChange(...args);
  }

  return (
    <Modal backdrop={'opaque'} hideCloseButton={locked} isOpen={isOpen}
           placement={'center'} scrollBehavior={'inside'} onOpenChange={onOpenChangeWrapper}>
      <ModalContent>
        {(onClose) => {
          return (
            <>
              <ModalHeader
                className="flex flex-col gap-1">{(editItem == null ? 'Add' : 'Modify') + ' item'}</ModalHeader>
              <ModalBody>
                <Input isDisabled={locked} label={'Name'} name={'name'}
                       value={item.name} onChange={handleOnChange} />
                <Input isDisabled={locked}
                       label={`Damage per tick ${numberInputInfo}`}
                       name={'damagePerTick'}
                       value={item.damagePerTick} onChange={handleOnChange} />
                <Input isDisabled={locked}
                       label={`Damage per use ${numberInputInfo}`}
                       name={'damagePerUse'}
                       value={item.damagePerUse} onChange={handleOnChange} />
              </ModalBody>
              <ModalFooter>
                <Button color="danger" isDisabled={locked} variant="light" onPress={onClose}>
                  Cancel
                </Button>
                <Button color="primary" isDisabled={locked} isLoading={locked} onPress={() => {
                  modifyItem(item);
                }}>
                  {editItem == null ? 'Add' : 'Modify'}
                </Button>
              </ModalFooter>
              <ErrorInfoModal message={errorMessage} setErrorMessage={setErrorMessage} />
            </>
          );
        }}
      </ModalContent>
    </Modal>
  );
}
