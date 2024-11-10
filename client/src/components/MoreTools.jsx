import React from 'react';
import { Card, CardBody } from '@nextui-org/card';

import AddItemButton from '@/components/button/AddItemButton';
import SimulateTimeButton from '@/components/button/SimulateTimeButton';

export default function MoreTools () {
  return (
    <>
      <div className={'flex flex-row justify-center'}>
        <Card className={'w-fit'}>
          <CardBody>
            <div className={'flex flex-row flex-wrap justify-center gap-2'}>
              <SimulateTimeButton />
              <AddItemButton />
            </div>
          </CardBody>
        </Card>
      </div>
    </>
  );
}
