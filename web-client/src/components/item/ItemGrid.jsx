import Item from '@/components/item/Item';
import { useGlobalStore } from '@/hooks/store/globalStore';

export default function ItemGrid () {
  const items = useGlobalStore((state) => state.items);

  return (
    <>
      <div className={'item-grid'}>
        {items.map((item) => (
          <Item key={item.id} item={item} />
        ))}
      </div>
    </>
  );
}
