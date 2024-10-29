namespace GameInv.ItemNS {
    public struct ItemDurability(ushort durability) {
        private readonly ushort _durability = durability;

        public const ushort MaxValue = ushort.MaxValue;
        public const ushort MinValue = ushort.MinValue;

        public static implicit operator ushort(ItemDurability itemDurability) {
            return itemDurability._durability;
        }
        public static implicit operator ItemDurability(ushort durability) {
            return new(durability);
        }
    }
}
