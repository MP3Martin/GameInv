namespace GameInv.UtilsNS.ErrorPresenterNS {
    public interface IErrorPresenter {
        public void Present(string message, Type? classType = null, bool pause = false);
    }
}
