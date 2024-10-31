using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GameInv.Ws {
    public static class MessageHandler {
        public static void HandleMessage(string message, WebSocketConnectionInterfaceWrapper socket, GameInv gameInv) {
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
                var item = ItemData.Deserialize(commandData);
                if (item is not null) {
                    gameInv.Inventory.AddItem(item!);
                    Success(true);
                    return;
                }

                Success(false, "Invalid item data");
            }

            void HandleModifyItem() {
                var item = ItemData.Deserialize(commandData);
                if (item is not null) {
                    gameInv.Inventory.ModifyItem(item!);
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
        }
    }
}
