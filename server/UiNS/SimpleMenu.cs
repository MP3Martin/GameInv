using static CarGarage.Ui.MenuPage;

namespace CarGarage.Ui {
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
