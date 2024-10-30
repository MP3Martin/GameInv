using System.Text;

namespace GameInv.Ws {
    public static class MessageData {
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
            commandData = Encoding.UTF8.GetString(Convert.FromBase64String(commandData));
            return true;
        }
        
        public static string Encode(string commandType, string messageUuid, string commandData) {
            return $"{commandType}|{messageUuid}|{Convert.ToBase64String(Encoding.UTF8.GetBytes(commandData))}";
        }
    }
}
