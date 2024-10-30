using Fleck;

namespace GameInv.Ws {
    public static class MessageHandler {
        public static void HandleMessage(string message, IWebSocketConnection socket, GameInv gameInv) {
            if (!DecodeMessage(message, out var commandType, out var messageUuid, out var commandData)) {
                Success(false, "Invalid message format");
                return;
            }

            var commandActions = new Dictionary<string, Action> {
                { "add_item", HandleAddItem },
                { "modify_item", HandleModifyItem },
                { "remove_item", HandleRemoveItem }
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

            void HandleAddItem() {
                if (ModifiedItem.Deserialize(commandData, out var itemId, out var item)) {
                    Success(true);
                    return;
                }

                Success(false, "Invalid item data");
            }

            void HandleModifyItem() {
                throw new NotImplementedException();
            }

            void HandleRemoveItem() {
                // ReSharper disable once InlineTemporaryVariable
                var itemId = commandData;
                gameInv.Inventory.RemoveItem(itemId);
                socket.Send(EncodeMessage((string)"Confirm", messageUuid, (string)"ok"));
            }
        }
    }
}
