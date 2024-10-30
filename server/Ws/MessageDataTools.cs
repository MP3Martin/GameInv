using System.ComponentModel.DataAnnotations;
using System.Text;
using GameInv.ItemNS;
using Newtonsoft.Json;

namespace GameInv.Ws {
    public static class MessageDataTools {
        public static bool Decode(string message, out string commandType, out string messageUuid, out string commandData) {
            commandType = "";
            messageUuid = "";
            commandData = "";

            var messageParts = message.Split('|');
            if (messageParts.Length != 3) return false;


            try {
                commandType = messageParts[0];
                messageUuid = messageParts[1];
                commandData = messageParts[2];
            } catch (IndexOutOfRangeException) { return false; }

            commandType = commandType.ToLower();
            messageUuid = messageUuid.ToLower();
            commandData = Encoding.UTF8.GetString(Convert.FromBase64String(commandData));
            return true;
        }

        public static string Encode(string commandType, string messageUuid, string commandData) {
            return $"{commandType.ToLower()}|{messageUuid}|{Convert.ToBase64String(Encoding.UTF8.GetBytes(commandData))}";
        }

        public static class ModifiedItem {
            /// <returns>Success</returns>
            public static bool Deserialize(string data, out string id, out Item? item) {
                id = "";
                item = null;

                var modifiedItemData = ModifiedItemData.Deserialize(data);
                if (modifiedItemData is null) return false;

                id = modifiedItemData.Id;
                item = modifiedItemData.ItemData;
                return true;
            }

            /// <returns>Null if unsuccessful</returns>
            public static string? Serialize(string id, Item item) {
                return new ModifiedItemData { Id = id, ItemData = item }.Serialize();
            }
        }

        /// <summary>
        /// Received from client
        /// </summary>
        [JsonObject(ItemRequired = Required.Always)]
        private class ModifiedItemData {
            public required string Id;
            public required ItemData ItemData;

            /// <returns>Null if unsuccessful</returns>
            public static ModifiedItemData? Deserialize(string data) {
                try {
                    return JsonConvert.DeserializeObject<ModifiedItemData>(data);
                } catch (JsonSerializationException) {
                    return null;
                }
            }

            /// <returns>Null if unsuccessful</returns>
            public string? Serialize() {
                try {
                    return JsonConvert.SerializeObject(this);
                } catch (JsonSerializationException) {
                    return null;
                }
            }
        }

        [JsonObject]
        private class ItemData {
            public int? DamagePerTick;
            public int? DamagePerUse;
            public int? Durability;
            [JsonRequired] public required string Name;

            public static implicit operator Item(ItemData data) {
                return new(data.Name, (ItemDurability?)data.DamagePerTick, (ItemDurability?)data.DamagePerUse, (ItemDurability?)data.Durability);
            }
            public static implicit operator ItemData(Item item) {
                return new ItemData {
                    DamagePerTick = item.DamagePerTick,
                    DamagePerUse = item.DamagePerUse,
                    Durability = item.Durability,
                    Name = item.Name
                };
            }
        }

        [JsonObject]
        public class SuccessData {
            [JsonRequired] public bool Success;
            public string? Message;

            public string? Serialize() {
                try {
                    return JsonConvert.SerializeObject(this);
                } catch (JsonSerializationException) {
                    return null;
                }
            }
        }
    }
}
