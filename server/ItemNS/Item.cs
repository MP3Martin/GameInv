namespace GameInv.ItemNS {
    public struct Item {
        public Item(string name, ItemDurability? damagePerTick = null, ItemDurability? damagePerUse = null) {
            Name = name;

            // Durability
            _damagePerTick = damagePerTick;
            _damagePerUse = damagePerUse;
            if (Usable || Decays) {
                _durability = new ItemDurability();
            }
        }

        public readonly string Name;
        private readonly ItemDurability? _damagePerTick;
        private readonly ItemDurability? _damagePerUse;
        private ItemDurability? _durability;
        public bool Usable => _damagePerUse is not null;
        public bool Decays => _damagePerTick is not null;

        /// <summary>
        /// Use the item and return the updated item or null if broken.
        /// </summary>
        public Item? Use() {
            if (!Usable) throw new InvalidOperationException("Item is not usable");
            return ApplyDurabilityChange(_damagePerUse!.Value);
        }

        /// <summary>
        /// Tick the item durability and return the updated item or null if broken.
        /// </summary>
        public Item? TickDurability() {
            if (!Decays) throw new InvalidOperationException("Item does not decay");
            return ApplyDurabilityChange(_damagePerTick!.Value);
        }

        /// <summary>
        /// Apply the specified durability change and return a new item or null if broken.
        /// </summary>
        private Item? ApplyDurabilityChange(ItemDurability damage) {

            var newDurability = _durability - damage;
            if (newDurability <= ItemDurability.MinValue) {
                return null;
            }

            var newItem = this;
            newItem._durability = new((ushort)newDurability!);
            return newItem;
        }
    }
}
