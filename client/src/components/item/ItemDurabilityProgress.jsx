import { Progress } from '@nextui-org/progress';

export default function ItemDurabilityProgress ({
  durabilityPercentage
}) {
  return (
    <>
      <Progress aria-label="Durability" className="max-w-md" value={durabilityPercentage}/>
    </>
  );
}
