using GameInv.ItemNS;
using MySql.Data.MySqlClient;

namespace GameInv.Db {
    public class MySqlItemDataSource : IItemDataSource {
        public string SourceName => "MySQL DB";
        public required string ConnectionString { get; init; }

        public IEnumerable<Item>? GetItems(out string? errorMessage) {
            errorMessage = null;
            try {
                using var connection = CreateAndOpenConnection();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM items";
                command.Prepare();

                using var reader = command.ExecuteReader();

                var items = new List<Item>();
                while (reader.Read()) {
                    items.Add(MapItem(reader));
                }

                return items;
            } catch (MySqlException ex) {
                errorMessage = ex.Message;
                return null;
            }
        }

        public bool UpdateItem(Item item) {
            try {
                using var connection = CreateAndOpenConnection();

                using var command = CreateUpsertCommand(connection);
                SetItemParameters(command, item);

                return command.ExecuteNonQuery() == 1;
            } catch (MySqlException) {
                return false;
            }
        }

        public bool UpdateItems(IEnumerable<Item> items) {
            try {
                items = items.ToArray();

                using var connection = CreateAndOpenConnection();

                using var transaction = connection.BeginTransaction();
                using var command = CreateUpsertCommand(connection, transaction);

                var updatedRows = 0;
                try {
                    foreach (var item in items) {
                        SetItemParameters(command, item);
                        updatedRows += command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                } catch {
                    transaction.Rollback();
                    throw;
                }

                return updatedRows == items.Count();
            } catch (MySqlException) {
                return false;
            }
        }

        public bool RemoveItem(Item item) {
            try {
                using var connection = CreateAndOpenConnection();

                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM items WHERE id=@id";
                command.Parameters.Add("@id", MySqlDbType.String, 36);
                command.Prepare();
                command.Parameters["@id"].Value = item.Id;

                return command.ExecuteNonQuery() == 1;
            } catch (MySqlException) {
                return false;
            }
        }

        public bool RemoveItems(IEnumerable<Item> items) {
            try {
                items = items.ToArray();

                using var connection = CreateAndOpenConnection();

                using var transaction = connection.BeginTransaction();
                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM items WHERE id=@id";
                command.Parameters.Add("@id", MySqlDbType.String, 36);
                command.Prepare();
                command.Transaction = transaction;

                var deletedRows = 0;
                try {
                    foreach (var item in items) {
                        command.Parameters["@id"].Value = item.Id;
                        deletedRows += command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                } catch {
                    transaction.Rollback();
                    throw;
                }

                return deletedRows == items.Count();
            } catch (MySqlException) {
                return false;
            }
        }

        private MySqlConnection CreateAndOpenConnection() {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        private static Item MapItem(MySqlDataReader reader) {
            return new(
                (string)reader["name"],
                GetNullableValue<ushort>(reader["damagePerTick"]),
                GetNullableValue<ushort>(reader["damagePerUse"]),
                GetNullableValue<ushort>(reader["durability"]),
                reader["id"].ToString()
            );
        }

        private static MySqlCommand CreateUpsertCommand(MySqlConnection connection, MySqlTransaction? transaction = null) {
            var command = connection.CreateCommand();
            if (transaction != null) command.Transaction = transaction;

            command.CommandText =
                """
                INSERT INTO items (name, damagePerTick, damagePerUse, durability, id)
                VALUES (@name, @damagePerTick, @damagePerUse, @durability, @id)
                ON DUPLICATE KEY UPDATE 
                name=@name, damagePerTick=@damagePerTick, damagePerUse=@damagePerUse, durability=@durability
                """;

            AddItemParameters(command);
            command.Prepare();

            return command;
        }

        private static void AddItemParameters(MySqlCommand command) {
            command.Parameters.Add("@name", MySqlDbType.VarChar, 70);
            command.Parameters.Add("@damagePerTick", MySqlDbType.UInt16);
            command.Parameters.Add("@damagePerUse", MySqlDbType.UInt16);
            command.Parameters.Add("@durability", MySqlDbType.UInt16);
            command.Parameters.Add("@id", MySqlDbType.String, 36);
        }

        private static void SetItemParameters(MySqlCommand command, Item item) {
            command.Parameters["@name"].Value = item.Name;
            command.Parameters["@damagePerTick"].Value = (ushort?)item.DamagePerTick;
            command.Parameters["@damagePerUse"].Value = (ushort?)item.DamagePerUse;
            command.Parameters["@durability"].Value = (ushort?)item.Durability;
            command.Parameters["@id"].Value = item.Id;
        }

        private static T? GetNullableValue<T>(object value) where T : struct {
            return value == DBNull.Value ? null : (T?)value;
        }
    }
}
