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
            <div className={'text-xl sm:text-4xl md:text-6xl w-full text-center'}>Not connected</div>
          </>}

      </DefaultLayout>
    </>
  );
}
