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

            /*
             *
             * TODO
             * TODO: send cars when the client joins, call itemschanged in Inventory.cs, send all cars to all clients
             * TODO: (subscribe to event in gameInv.inventory)
             * TODO
             * 
             */
            
            void Success(bool success, string? infoMessage = null) {
                socket.Send(EncodeMessage((string)"confirm", messageUuid, new SuccessData {
                        Success = success, Message = infoMessage
                    }.Serialize()!)
                );
            }

            void HandleAddItem() {
                if (ModifiedItem.Deserialize(commandData, out var item)) {
                    gameInv.Inventory.AddItem(item!);
                    Success(true);
                    return;
                }

                Success(false, "Invalid item data");
            }

            void HandleModifyItem() {
                if (ModifiedItem.Deserialize(commandData, out var item)) {
                    gameInv.Inventory.ModifyItem(item!);
                    Success(true);
                    return;
                }

                Success(false, "Invalid item data");            }

            void HandleRemoveItem() {
                // ReSharper disable once InlineTemporaryVariable
                var itemId = commandData;
                gameInv.Inventory.RemoveItem(itemId);
                socket.Send(EncodeMessage((string)"Confirm", messageUuid, (string)"ok"));
            }
        }
    }
}
