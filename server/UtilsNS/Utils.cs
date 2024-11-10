using System.Diagnostics;
using System.Reflection;
using GameInv.ConsoleUiNS;
using GameInv.ConsoleUiNS.Menus.SimpleMenus;
using GameInv.ItemNS;

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
        public static bool YesNoInput(string? prompt = null, bool? defaultAnswer = null) {
            Console.WriteLine((prompt ?? "") +
                $" [{(defaultAnswer ?? false ? "Y" : "y")}/{(defaultAnswer ?? true ? "n" : "N")}]");
            Console.CursorVisible = false;

            try {
                while (true) {
                    var key = Console.ReadKey(true).Key;

                    if (key is ConsoleKey.Y or ConsoleKey.N) {
                        return key == ConsoleKey.Y;
                    }

                    if (defaultAnswer is not null && key == ConsoleKey.Enter) {
                        return (bool)defaultAnswer;
                    }
                }
            } finally {
                Console.CursorVisible = true;
            }
        }

        public static void Pause(ConsoleKey? key = null, bool newLine = false) {
            if (newLine) Console.Write(Environment.NewLine);
            Console.Write($"Press {key ?? (object)"any key"} to continue . . . ");
            if (key is null) {
                Console.ReadKey(true);
            } else {
                while (Console.ReadKey(true).Key != key) { }
            }
        }

        /// <summary>
        ///     Tries to get the user input until it can be cast into T and returns it.
        /// </summary>
        /// <returns><b>null</b> if <paramref name="emptyReturnsNull" /> is true and the user didn't input anything</returns>
        public static T? PromptParse<T>(string prompt, bool emptyReturnsNull, Func<T, bool>? condition = null) where T : struct {
            while (true) {
                var input = Prompt(prompt);
                if (emptyReturnsNull && input == string.Empty) return null;
                if (TryParse(input, out T output) && (condition?.Invoke(output) ?? true)) {
                    return output;
                }

                JumpToPrevLineClear();
            }
        }

        /// <summary>
        ///     Tries to get the user input until it can be cast into T and returns it.
        /// </summary>
        public static T PromptParse<T>(string prompt, Func<T, bool>? condition = null) where T : struct {
            return (T)PromptParse(prompt, false, condition)!;
        }

        public static (string, Action)[] ItemsAsMenuOptions(IEnumerable<Item> items, Action<Item> onSelect) {
            return items.Select<Item, (string, Action)>(x =>
                (x.Name, () => {
                    onSelect(x);
                })
            ).ToArray();
        }

        #region From Jez @ SO
        // Thanks to https://stackoverflow.com/users/178757/jez @ https://stackoverflow.com/a/26055541/10518428
        // Code in this region is not written by me

        /// <summary>
        ///     Tries to convert the specified string representation of a logical value to
        ///     its type T equivalent. A return value indicates whether the conversion
        ///     succeeded or failed.
        /// </summary>
        /// <typeparam name="T">The type to try and convert to.</typeparam>
        /// <param name="value">A string containing the value to try and convert.</param>
        /// <param name="result">If the conversion was successful, the converted value of type T.</param>
        /// <returns>If value was converted successfully, true; otherwise false.</returns>
        private static bool TryParse<T>(string value, out T result) where T : struct {
            var tryParseMethod = typeof(T).GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null,
                new[] { typeof(string), typeof(T).MakeByRefType() }, null);
            var parameters = new object[] { value, null! };

            var retVal = (bool)tryParseMethod!.Invoke(null, parameters)!;

            result = (T)parameters[1];
            return retVal;
        }

        /// <summary>
        ///     Tries to convert the specified string representation of a logical value to
        ///     its type T equivalent. A return value indicates whether the conversion
        ///     succeeded or failed.
        /// </summary>
        /// <typeparam name="T">The type to try and convert to.</typeparam>
        /// <param name="value">A string containing the value to try and convert.</param>
        /// <returns>If value was converted successfully, true; otherwise false.</returns>
        public static bool TryParse<T>(string value) where T : struct {
            var retVal = TryParse(value, out T _);
            return retVal;
        }
        #endregion
    }
}
