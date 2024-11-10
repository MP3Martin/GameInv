declare global {
  interface Item {
    name: string;
    id: string;
    damagePerTick: number | null;
    damagePerUse: number | null;
    durability: number | null;
  }
}
export {};
