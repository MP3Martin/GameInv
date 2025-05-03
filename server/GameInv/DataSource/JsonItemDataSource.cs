using System.Text;
using GameInv.ItemNS;
using Newtonsoft.Json;

namespace GameInv.DataSource {
    public class JsonItemDataSource : IItemDataSource {
        private static readonly Logger Log = GetLogger();
        private readonly GameInv _gameInv;
        private readonly ResettableCountdownTimer _saveTimer;
        private bool _saving;
        private string _storagePath = null!;

        public JsonItemDataSource(GameInv gameInv) {
            _gameInv = gameInv;
            _saveTimer = new(SaveData, 1000);

            _gameInv.Closing += MoreGuaranteedSaveData;
        }

        public string SourceName => "JSON file";

        public IEnumerable<Item>? GetItems(out string? errorMessage) {
            errorMessage = null;
            void LogFileLocationInfo() {
                Log.Info($"{SourceName} location: {_storagePath}");
            }

            try {
                const string defaultFileName = "GameInv.json";
                _storagePath = MyEnv.GetString("STORAGE_FILE_PATH") ?? defaultFileName;
                _storagePath = Path.GetFullPath(_storagePath, AppDomain.CurrentDomain.BaseDirectory);
                if (Directory.Exists(_storagePath)) {
                    _storagePath = Path.Combine(_storagePath, defaultFileName);
                }

                if (!File.Exists(_storagePath)) {
                    Log.Info($"{SourceName} doesn't exist, creating.");
                    File.WriteAllText(_storagePath, "[]");
                    LogFileLocationInfo();
                    return [];
                }

                LogFileLocationInfo();

                var saveData = File.ReadAllText(_storagePath, Encoding.UTF8);
                var itemsAsItemData = JsonConvert.DeserializeObject<ItemData[]>(saveData);
                var items = itemsAsItemData?.Select(i => (Item)i).ToArray();
                return items;
            } catch (Exception ex) {
                errorMessage = FormatException(ex);
                return null;
            }
        }

        public bool UpdateItem(Item item) {
            return ResetSaveTimer();
        }

        public bool UpdateItems(IEnumerable<Item> items) {
            return ResetSaveTimer();
        }

        public bool RemoveItem(Item item) {
            return ResetSaveTimer();
        }

        public bool RemoveItems(IEnumerable<Item> items) {
            return ResetSaveTimer();
        }

        private void SaveData() {
            if (_saving) return;
            _saving = true;
            Log.Info("Started saving...");
            try {
                var itemsAsItemData = _gameInv.Inventory.Select(i => (ItemData)i).ToArray();
                var saveData = JsonConvert.SerializeObject(itemsAsItemData);
                File.WriteAllText(_storagePath, saveData);
                Log.Info($"Saved data to {SourceName}");
            } catch (Exception ex) {
                _gameInv.ErrorPresenter.Present("Error saving data.\n\n" + FormatException(ex));
            } finally {
                _saving = false;
            }
        }

        /// <summary>
        ///     Tries to wait for previous saving to finish and then calls <see cref="SaveData" />.
        ///     Gives up after 5 seconds.
        /// </summary>
        private void MoreGuaranteedSaveData() {
            const int timeoutSeconds = 5;
            if (SpinWait.SpinUntil(() => _saving == false, timeoutSeconds * 1000)) {
                SaveData();
            } else {
                _gameInv.ErrorPresenter.Present($"Previous save operation didn't finish " +
                    $"in {timeoutSeconds}+ seconds. Some data may be lost.");
            }
        }

        private bool ResetSaveTimer() {
            _saveTimer.Reset();
            return true;
        }
    }
}
