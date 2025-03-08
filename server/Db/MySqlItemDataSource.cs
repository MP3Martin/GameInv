using GameInv.ItemNS;
using MySql.Data.MySqlClient;

namespace GameInv.Db {
    public class MySqlItemDataSource : IItemDataSource {
        public required string ConnectionString { get; init; }

        public IEnumerable<Item> GetItems() {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM items";
            using var data = command.ExecuteReader();

            var result = new List<Item>();
            while (data.Read()) {
                var item = new Item(
                    (string)data["name"],
                    (ItemDurability)data["damagePerTick"],
                    (ItemDurability)data["damagePerUse"],
                    (ItemDurability)data["durability"],
                    (string)data["id"]
                );
                result.Add(item);
            }

            return result;
        }

        public bool UpdateItem(Item item) {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO items VALUES (@name, @damagePerTick, @damagePerUse, @durability, @id) ON DUPLICATE KEY UPDATE";
            command.Parameters.Add("@name", MySqlDbType.VarChar, 70).Value = item.Name;
            command.Parameters.Add("@damagePerTick", MySqlDbType.UInt16).Value = item.DamagePerTick;
            command.Parameters.Add("@damagePerUse", MySqlDbType.UInt16).Value = item.DamagePerUse;
            command.Parameters.Add("@durability", MySqlDbType.UInt16).Value = item.Durability;
            command.Parameters.Add("@id", MySqlDbType.String, 36).Value = item.Id;

            command.Prepare();
            command.Parameters["@name"].Value = item.Name;
            command.Parameters["@damagePerTick"].Value = item.DamagePerTick;
            command.Parameters["@damagePerUse"].Value = item.DamagePerUse;
            command.Parameters["@durability"].Value = item.Durability;
            command.Parameters["@id"].Value = item.Id;

            return command.ExecuteNonQuery() == 1;
        }

        public bool RemoveItem(Item item) {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM items WHERE id=@id";
            command.Parameters.Add("@id", MySqlDbType.String, 36).Value = item.Id;

            command.Prepare();
            command.Parameters["@id"].Value = item.Id;

            return command.ExecuteNonQuery() == 1;
        }
    }
}
