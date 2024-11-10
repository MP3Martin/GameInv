import { Card, CardFooter, CardHeader } from '@nextui-org/card';
import { Divider } from '@nextui-org/divider';

import MoreOptionsButton from '@/components/button/MoreOptionsButton';
import UseItemButton from '@/components/button/UseItemButton';
import { siteConfig } from '@/config/consts/site';

/** @param text {string}
 * @param formatter {(value: number) => string | null}
 * @param valueParam {number} */
function itemProperty (text, valueParam, formatter = null) {
  // Do not render properties with no value
  if (valueParam == null) return;

  const value = formatter === null ? valueParam : formatter(valueParam);

  return <p className="text-small text-default-500">{text}: <span className="text-default-700">{value}</span></p>;
}

/** @param item {global.Item} */
export default function Item ({
  item
}) {
  return (
    <Card className="w-full h-fit">
      <CardHeader className="flex gap-3">
        {/* <Image */}
        {/*   alt="nextui logo" */}
        {/*   height={40} */}
        {/*   radius="sm" */}
        {/*   src="https://avatars.githubusercontent.com/u/86160567?s=200&v=4" */}
        {/*   width={40} */}
        {/* /> */}
        <div className="flex flex-col">
          <p className="text-lg min-h-4">{item.name.trim() ? item.name : (<>&nbsp;</>)}</p>
          {itemProperty('Durability', item.durability, value => parseFloat(((value / siteConfig.ushortMax) * 100).toFixed(3)) + '%')}
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
