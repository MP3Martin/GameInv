namespace GameInv_WPF.ItemNS {
    public readonly struct ItemDurability : IComparable<ItemDurability> {
        private readonly ushort _durability;

        public const ushort MaxValue = ushort.MaxValue;
        public const ushort MinValue = ushort.MinValue;

        // Thanks to Matt Jenkins @ https://stackoverflow.com/a/78287646/10518428
        [Obsolete("You may not use the parameterless constructor.", true)]
        public ItemDurability() {
            throw new InvalidOperationException("You may not use the parameterless constructor.");
        }

        public ItemDurability(ushort? durability = null) {
            if (durability is not null) {
                _durability = (ushort)durability;
            } else {
                _durability = MaxValue;
            }
        }

        public static implicit operator ushort(ItemDurability itemDurability) {
            return itemDurability._durability;
        }
        public static implicit operator ItemDurability(ushort durability) {
            return new(durability);
        }

        public override string ToString() {
            return _durability.ToString();
        }
        public int CompareTo(ItemDurability other) {
            return _durability.CompareTo(other._durability);
        }
    }
}
