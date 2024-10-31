using System.Diagnostics;
using CarGarage.Ui;
using CarGarage.Ui.Menus.SimpleMenus;

namespace GameInv.UtilsNS {
    public static class Utils {
        /// <summary>
        ///     Do not use this outside of a class constructor. <br />
        ///     Basically do not use this unless you are absolutely sure you know what you are doing.
        ///     <br /><br />
        ///     This exists only because I am lazy to do <c>Logger.GetLogger(typeof(ClassName))</c> because
        ///     <c>ClassName</c> is different in each class.
        /// </summary>
        public static Logger GetLogger() {
            // Gets the constructor (because GetLogger (this method) gets called in it)
            var constructor = new StackTrace().GetFrame(1)!.GetMethod()!;

            // Gets the class that contains the constructor
            var classType = constructor.DeclaringType!;

            return Logger.GetLogger(classType);
        }
        
        public static string Prompt(string prompt) {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }
        
        public static void JumpToPrevLineClear(int lineCount = 1) {
            foreach (var _ in Enumerable.Range(0, lineCount)) {
                Console.CursorTop--;
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.BufferWidth - 1));
                Console.CursorLeft = 0;
            }
        }
        
        /// <summary>
        ///     Fully clears the console, including the history
        /// </summary>
        public static void ClearAll() {
            Console.Clear();
            Console.Write("\x1b[3J");
        }
        
        public static void ShowMenu<T>() where T : IMenu, new() {
            new T().Show();
        }

        public static void ShowInfo(string message) {
            new InfoSimpleMenu(message).Show();
        }
        
        /// <summary>
        ///     A yes/no input with optional default yes/no
        /// </summary>
        /// <returns>
        ///     Yes -> <c>true</c><br />
        ///     No -> <c>false</c>
        /// </returns>
        public static bool YesNoInput(string? prompt = null, bool? yesIsDefault = null) {
            Console.WriteLine((prompt ?? "") +
                $" [{(yesIsDefault ?? false ? "Y" : "y")}/{(yesIsDefault ?? true ? "n" : "N")}]");
            Console.CursorVisible = false;

            try {
                while (true) {
                    var key = Console.ReadKey(true).Key;

                    if (key is ConsoleKey.Y or ConsoleKey.N) {
                        return key == ConsoleKey.Y;
                    }

                    if (yesIsDefault is not null && key == ConsoleKey.Enter) {
                        return (bool)yesIsDefault;
                    }
                }
            } finally {
                Console.CursorVisible = true;
            }
        }
    }
}
