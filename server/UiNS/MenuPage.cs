using Pastel;
using static System.Drawing.Color;

namespace CarGarage.Ui {
    /// <remarks>
    ///     You have to put some stuff into <see cref="Options" />
    /// </remarks>
    public abstract class MenuPage : IMenu {
        private readonly (string, Action)[] _options = [];
        private bool _shouldExit;

        private bool _shownOnce;

        protected (string, Action)[] Options {
            get => _options;
            init {
                _options = value;
                (string, Action) goBackItem = (GoBackMenuString, ExitMenu);
                _options = _options.Length switch {
                    > 0 when _options[^1].Item1 != ExitMenuString => _options.Prepend(goBackItem).ToArray(),
                    0 => [goBackItem],
                    _ => _options
                };
            }
        }
        protected abstract string Title { get; }

        public void Show() {
            if (_shownOnce) return;
            _shownOnce = true;

            while (!_shouldExit) {
                HandleChoiceInput();
            }
        }

        /// <summary>
        ///     Exits the menu. Should be called from the menu item that exits the menu.
        /// </summary>
        protected void ExitMenu() {
            _shouldExit = true;
        }

        public static void RenderTitle(string title) {
            ClearAll();
            Console.WriteLine(title.Pastel(DeepSkyBlue) + "\n");
        }

        private void RenderScreen() {
            RenderTitle(Title);
            for (var i = 0; i < Options.Length; i++) {
                var optionName = Options[i].Item1;
                if (optionName is ExitMenuString or GoBackMenuString) {
                    optionName = optionName.Pastel(ExitMenuColor);
                }

                Console.WriteLine($"{i + 1}. ".Pastel(MediumPurple) + optionName);
            }

            Console.WriteLine();
        }

        private void HandleChoiceInput() {
            RenderScreen();

            var ok = false;
            var choiceIndex = 0;
            while (!ok) {
                var stringChoice = Prompt("Choice: ");
                if (!int.TryParse(stringChoice, out var intChoice)) {
                    JumpToPrevLineClear();
                    continue;
                }

                choiceIndex = intChoice - 1;
                if (choiceIndex >= 0 && choiceIndex < Options.Length) {
                    ok = true;
                } else {
                    JumpToPrevLineClear();
                }
            }

            Options[choiceIndex].Item2.Invoke();
        }
    }
}
