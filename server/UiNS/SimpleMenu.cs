using static GameInv.UiNS.MenuPage;

namespace GameInv.UiNS {
    public abstract class SimpleMenu : IMenu {
        private bool _shownOnce;

        protected abstract string Title { get; }

        public void Show() {
            if (_shownOnce) return;
            _shownOnce = true;

            RenderTitle(Title);
            OnShow();
        }

        protected abstract void OnShow();
    }
}
