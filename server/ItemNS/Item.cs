namespace GameInv.ItemNS {
    public struct Item(string name) {
        public readonly ItemDurability DamagePerTick;
        public readonly ItemDurability DamagePerUse;
        public readonly ItemDurability Durability = new();
        public string Name = name;
        
        // todo: only store durability if needed (also store a bool), make them nullable and only define in the contructor if bool is true
    }
}
