import { Link } from '@nextui-org/link';

import DefaultLayout from '@/layouts/default';
import { WebSocket } from '@/components/WebSocket';
import ItemGrid from '@/components/item/ItemGrid';
import MoreTools from '@/components/MoreTools';
import { useGlobalStore } from '@/hooks/store/globalStore';
import { wsStatus as wsStatusEnum } from '@/config/consts/enums';

export default function IndexPage () {
  const wsStatus = useGlobalStore((state) => state.wsStatus);

  return (
    <>
      <WebSocket />
      <DefaultLayout>
        {/* {[...Array(2).keys()].map((i) => ( */}
        {/*   <p key={i}> */}
        {/*     Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam */}
        {/*     pulvinar risus non risus hendrerit venenatis. Pellentesque sit amet */}
        {/*     hendrerit risus, sed porttitor quam. */}
        {/*   </p> */}
        {/* ))} */}

        {(wsStatus === wsStatusEnum.open)
          ? <>
            <MoreTools />
            <div className={'my-[1.5rem]'} />
            <ItemGrid />
          </>
          : <>
            <div className={'flex flex-col items-center'}>
              <p className={'text-4xl md:text-6xl mb-1'}>
                Not connected
              </p>
              <p className={'text-xl'}>
                Please download the GameInv server&nbsp;
                <Link isExternal className={'text-[100%]'}
                      href={'https://github.com/MP3Martin/GameInv#console--web-ui'}>here</Link>
              </p>
            </div>
          </>}

      </DefaultLayout>
    </>
  );
}
