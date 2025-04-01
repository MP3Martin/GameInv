namespace GameInv.UtilsNS.ErrorPresenterNS {
    public class ConsoleErrorPresenter : IErrorPresenter {
        private static readonly Logger Log = GetLogger();
        public void Present(string message, Type? classType = null, bool pause = false) {
            if (pause) Log.LogLevel = LogLevel.Fatal;
            Console.WriteLine(message);
            if (pause) Pause(newLine: true);
        }
    }
}
