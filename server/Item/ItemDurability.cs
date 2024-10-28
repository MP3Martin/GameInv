namespace GameInv {
    public struct ItemDurability(ushort durability) {
        private ushort _durability = durability;

        public ushort MaxValue = ushort.MaxValue;
        public ushort MinValue = ushort.MinValue;
    }
}
