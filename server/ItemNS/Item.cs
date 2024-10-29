namespace GameInv.ItemNS {
    public struct Item {
        public Item(string name, ItemDurability? damagePerTick = null, ItemDurability? damagePerUse = null) {
            Name = name;

            // Durability
            _damagePerTick = damagePerTick;
            _damagePerUse = damagePerUse;
            if (damagePerTick is not null || damagePerUse is not null) {
                _durability = new ItemDurability();
            }
        }

        public readonly string Name;
        private readonly ItemDurability? _damagePerTick;
        private readonly ItemDurability? _damagePerUse;
        private ItemDurability? _durability;
        public bool Usable => _damagePerUse is not null;
        public bool Decays => _damagePerTick is not null;

        /// <returns>
        /// <ul>
        ///     <li>The item</li>
        ///     <li>Null if the item was fully broken</li>
        /// </ul>
        /// </returns>
        public Item? Use() {
            if (!Usable) throw new InvalidOperationException("Item is not usable");
            var newDurability = _durability - _damagePerUse;
            if (newDurability <= ItemDurability.MinValue) {
                return null;
            }

            var newItem = this;
            newItem._durability = new((ushort)newDurability!);
            return newItem;
        }

        /// <returns>
        /// <ul>
        ///     <li>The item</li>
        ///     <li>Null if the item was fully broken</li>
        /// </ul>
        /// </returns>
        public Item? TickDurability() {
            if (!Decays) throw new InvalidOperationException("Item does not decay");
            var newDurability = _durability - _damagePerTick;
            if (newDurability <= ItemDurability.MinValue) {
                return null;
            }

            var newItem = this;
            newItem._durability = new((ushort)newDurability!);
            return newItem;
        }
    }
}
