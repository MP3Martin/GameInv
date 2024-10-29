namespace GameInv.ItemNS {
    public class Item(string name) {
        public ItemDurability DamagePerTick;
        public ItemDurability DamagePerUse;
        public ItemDurability Durability = new();
        public string Name = name;
    }
}
