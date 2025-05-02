namespace GameInv.UtilsNS.ErrorPresenter {
    public class ConsoleErrorPresenter : IErrorPresenter {
        public void Present(string message, bool pause = false) {
            var log = GetLogger(GetCallerClassType());

            log.Error("\n" + message);

            if (!pause) return;
            log.LogLevel = LogLevel.Fatal; // Disable logging
            Pause(newLine: true);
        }
    }
}
