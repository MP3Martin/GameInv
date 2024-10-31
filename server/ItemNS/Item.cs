using System.Text;
using Pastel;

namespace GameInv.ItemNS {
    public class Item {
        public readonly ItemDurability? DamagePerTick;
        public readonly ItemDurability? DamagePerUse;
        public readonly string Id;
        public readonly string Name;
        public Item(string name, ItemDurability? damagePerTick = null, ItemDurability? damagePerUse = null, ItemDurability? durability = null,
            string? id = null) {
            Name = name;
            Id = id ?? Guid.NewGuid().ToString();

            // Durability
            DamagePerTick = damagePerTick;
            DamagePerUse = damagePerUse;
            if ((Usable || Decays) && durability is null) {
                Durability = new ItemDurability(null);
            }
        }
        public ItemDurability? Durability { get; private set; }
        public bool Usable => DamagePerUse is not null;
        public bool Decays => DamagePerTick is not null;

        /// <summary>
        ///     Use the item and return the updated item or null if broken.
        /// </summary>
        public Item? Use() {
            if (!Usable) throw new InvalidOperationException("Item is not usable");
            return ApplyDurabilityChange(DamagePerUse!.Value);
        }

        /// <summary>
        ///     Tick the item durability and return the updated item or null if broken.
        /// </summary>
        public Item? TickDurability(int ticks) {
            if (!Decays) throw new InvalidOperationException("Item does not decay");
            return ApplyDurabilityChange((ItemDurability)(DamagePerTick!.Value * ticks));
        }

        /// <summary>
        ///     Apply the specified durability change and return a new item or null if broken.
        /// </summary>
        private Item? ApplyDurabilityChange(ItemDurability damage) {
            var newDurability = Durability - damage;
            if (newDurability <= ItemDurability.MinValue) {
                return null;
            }

            var newItem = this;
            newItem.Durability = new((ushort)newDurability!);
            return newItem;
        }

        public override string ToString() {
            var additionalFields = new List<(string name, string value)>();

            if (Decays) additionalFields.Add(("Damage per game tick", ((ushort)DamagePerTick!).ToString()));
            if (Usable) additionalFields.Add(("Damage per use", ((ushort)DamagePerUse!).ToString()));
            if (Durability is not null) additionalFields.Add(("Durability", (ushort)Durability! + " / " + ItemDurability.MaxValue));

            var result = new StringBuilder();

            var fieldNameColor = DodgerBlue;
            var fieldValueColor = DarkOrange;

            result.AppendLine("Name".Pastel(fieldNameColor) + ": " + Name.Pastel(fieldValueColor));
            foreach (var additionalField in additionalFields) {
                result.AppendLine($"  {additionalField.name.Pastel(fieldNameColor)}: {additionalField.value.Pastel(fieldValueColor)}");
            }

            return result.ToString();
        }
    }
}
