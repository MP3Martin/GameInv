using GameInv.UtilsNS.ErrorPresenterNS;

namespace GameInv_WPF.UtilsNS {
    public class MessageBoxErrorPresenter : IErrorPresenter {
        public void Present(string message, Type? classType = null, bool pause = false) {
            var log = GetLogger(GetCallerClassType());

            log.Error(message);
            ShowErrorMessageBox(message);
        }
    }
}
