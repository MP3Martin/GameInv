namespace GameInv.Ws {
    public static class MessageHandler {
        public static void HandleMessage(string message, WebSocketConnectionInterfaceWrapper socket, GameInv gameInv) {
            if (!DecodeMessage(message, out var commandType, out var messageUuid, out var commandData)) {
                Success(false, "Invalid message format");
                return;
            }

            var commandActions = new Dictionary<string, Action> {
                { "modify_item", HandleModifyItem },
                { "remove_item", HandleRemoveItem },
                { "use_item", HandleUseItem },
                { "tick_time", HandleTickTime }
            };

            if (commandActions.TryGetValue(commandType, out var action)) {
                action.Invoke();
            } else {
                Success(false, "Invalid command type");
            }

            return;

            void Success(bool success, string? infoMessage = null) {
                socket.Send(EncodeMessage((string)"confirm", messageUuid, new SuccessData {
                        Success = success, Message = infoMessage
                    }.Serialize()!)
                );
            }

            void HandleModifyItem() {
                var item = ItemData.Deserialize(commandData);
                if (item is not null) {
                    // Check the item data
                    if (string.IsNullOrEmpty(item.Name)) {
                        Success(false, "Name not specified");
                        return;
                    }

                    if (gameInv.Inventory.GetItemIndex(item.Id) == -1) {
                        gameInv.Inventory.AddItem(item);
                    } else {
                        gameInv.Inventory.ModifyItem(item);
                    }

                    Success(true);
                    return;
                }

                Success(false, "Invalid item data");
            }

            void HandleRemoveItem() {
                // ReSharper disable once InlineTemporaryVariable
                var itemId = commandData;
                Success(gameInv.Inventory.RemoveItem(itemId));
            }
            void HandleUseItem() {
                // ReSharper disable once InlineTemporaryVariable
                var itemId = commandData;
                Success(gameInv.Inventory.UseItem(itemId, out _));
            }
            void HandleTickTime() {
                var success = int.TryParse(commandData, out var tickCount);
                if (!success) {
                    Success(false, "Invalid tick count");
                    return;
                }

                Success(gameInv.Inventory.TickTime(tickCount));
            }
        }
    }
}
