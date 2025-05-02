using GameInv.UtilsNS.ErrorPresenter;

namespace GameInv_WPF.UtilsNS {
    public class MessageBoxErrorPresenter : IErrorPresenter {
        public void Present(string message, bool pause = false) {
            var log = GetLogger(GetCallerClassType());

            log.Error("\n" + message);
            ShowErrorMessageBox(message);
        }
    }
}
