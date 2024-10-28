namespace GameInv {
    public class Item(string name) {
        public string Name = name;
        public ItemDurability Durability = new();
        public bool DamagePerTick
    }
}
