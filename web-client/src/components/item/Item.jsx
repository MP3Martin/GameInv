import { Card, CardFooter, CardHeader } from '@nextui-org/card';
import { Divider } from '@nextui-org/divider';

import MoreOptionsButton from '@/components/button/MoreOptionsButton';
import UseItemButton from '@/components/button/UseItemButton';
import { siteConfig } from '@/config/consts/site';
import { durabilityPercentage } from '@/config/utils/utils';
import ItemDurabilityProgress from '@/components/item/ItemDurabilityProgress';

/** @param text {string}
 * @param formatter {(value: number) => string | null}
 * @param additionalComponent {(value: any) => JSX.Element}
 * @param valueParam {number} */
function itemProperty (text, valueParam, formatter = null, additionalComponent = null) {
  // Do not render properties with no value
  if (valueParam == null) return;

  const value = formatter === null ? valueParam : formatter(valueParam);

  return <>
    <p className="text-small text-default-500">{text}: <span className="text-default-700">{value}</span></p>
    {(additionalComponent !== null) && additionalComponent(valueParam)}
  </>;
}

/** @param item {global.Item} */
export default function Item ({
  item
}) {
  return (
    <Card className="w-full h-fit">
      <CardHeader className="flex gap-3">
        <div className="flex flex-col">
          <p
            className="text-lg min-h-4 wrap [overflow-wrap:anywhere]">{item.name.trim() ? item.name : (<>&nbsp;</>)}</p>
          {itemProperty('Durability', item.durability, value => parseFloat((durabilityPercentage(value)).toFixed(3)) + '%', (value) =>
            <ItemDurabilityProgress durability={value} />)}
          {itemProperty('Damage per tick', item.damagePerTick, value => (value + ' / ' + siteConfig.ushortMax))}
          {itemProperty('Damage per use', item.damagePerUse, value => (value + ' / ' + siteConfig.ushortMax))}
        </div>
      </CardHeader>
      {/* <Divider /> */}
      {/* <CardBody> */}
      {/*   <p className={'text-small'}>Lorem ipsum.</p> */}
      {/* </CardBody> */}
      <Divider />
      <CardFooter>
        <div className={'flex flex-row gap-3'}>
          <UseItemButton item={item} />
          <MoreOptionsButton item={item} />
        </div>
      </CardFooter>
    </Card>
  );
}
