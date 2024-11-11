import { Progress } from '@nextui-org/progress';
import styled from 'styled-components';

import { durabilityPercentage, percentageToDamageColor } from '@/config/utils/utils';

export default function ItemDurabilityProgress ({
  durability
}) {
  const percentage = durabilityPercentage(durability);

  const ProgressColorProvider = styled.div`
      & + div > div > div {
          background-color: ${percentageToDamageColor(percentage)};
      }
  `;

  return (
    <>
      <div className={'pb-2'}>
        <ProgressColorProvider />
        <Progress aria-label="Durability" className="container"
                  size="sm"
                  value={percentage} />
      </div>
    </>
  );
}
