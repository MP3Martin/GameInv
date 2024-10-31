using System.Text;
using GameInv.ItemNS;
using Newtonsoft.Json;

namespace GameInv.Ws {
    public static class MessageDataTools {
        /// <returns>Success</returns>
        public static bool DecodeMessage(string message, out string commandType, out string messageUuid, out string commandData) {
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
            try {
                commandData = Encoding.UTF8.GetString(Convert.FromBase64String(commandData));
            } catch (FormatException) { return false; }

            return true;
        }

        public static string EncodeMessage(string commandType, string? messageUuid, string commandData) {
            messageUuid ??= Guid.NewGuid().ToString();
            return $"{commandType.ToLower()}|{messageUuid}|{Convert.ToBase64String(Encoding.UTF8.GetBytes(commandData))}";
        }

        [JsonObject]
        public class ItemData {
            public ushort? DamagePerTick;
            public ushort? DamagePerUse;
            public ushort? Durability;
            [JsonRequired] public required string Id;
            [JsonRequired] public required string Name;

            public static implicit operator Item(ItemData data) {
                return new(data.Name, data.DamagePerTick, data.DamagePerUse, data.Durability, data.Id);
            }
            public static implicit operator ItemData(Item item) {
                return new() {
                    DamagePerTick = item.DamagePerTick,
                    DamagePerUse = item.DamagePerUse,
                    Durability = item.Durability,
                    Name = item.Name,
                    Id = item.Id
                };
            }

            /// <returns>Success</returns>
            public static Item? Deserialize(string data) {
                try {
                    var result = JsonConvert.DeserializeObject<ItemData>(data);
                    if (result is not null) {
                        return (Item)result;
                    }
                } catch (JsonSerializationException) { }

                return null;
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
        public class SuccessData {
            public string? Message;
            [JsonRequired] public required bool Success;

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
