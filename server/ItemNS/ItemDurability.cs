namespace GameInv.ItemNS {
    public struct ItemDurability(ushort durability) {
        private readonly ushort _durability = durability;

        public const ushort MaxValue = ushort.MaxValue;
        public const ushort MinValue = ushort.MinValue;

        public static implicit operator ushort(ItemDurability itemDurability) => itemDurability._durability;
    }
}
