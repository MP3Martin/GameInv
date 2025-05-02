namespace GameInv.UtilsNS.ErrorPresenter {
    public interface IErrorPresenter {
        public void Present(string message, bool pause = false);
    }
}
