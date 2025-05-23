using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using GameInv.ConsoleUiNS.Menus.SimpleMenus;
using GameInv.DataSource;
using GameInv.ItemNS;
using GameInv.UtilsNS.ErrorPresenter;
using Pastel;
using Color = System.Drawing.Color;

namespace GameInv.UtilsNS {
    public static partial class Utils {
        /// <summary>
        ///     Do not use this outside of a class constructor. <br />
        ///     Basically do not use this unless you are absolutely sure you know what you are doing.
        ///     <br /><br />
        ///     This exists only because I am lazy to do <c>Logger.GetLogger(typeof(ClassName))</c> because
        ///     <c>ClassName</c> is different in each class.
        /// </summary>
        public static Logger GetLogger(Type? classType = null) {
            classType ??= GetCallerClassType();
            return Logger.GetLogger(classType);
        }

        public static void InitLogger() {
            var logLevelColorMap = new Dictionary<LogLevel, Color> {
                { LogLevel.Trace, Cyan },
                { LogLevel.Debug, Blue },
                { LogLevel.Info, White },
                { LogLevel.Warn, Orange },
                { LogLevel.Error, Red },
                { LogLevel.Fatal, Magenta }
            };

            Logger.AddAppender((logger, level, message) => {
                message = "[".Pastel(MiscChar) +
                    DateTime.Now.ToString(LogTimeFormat).Pastel(LessImportantText) +
                    " " +
                    level.ToString().Pastel(logLevelColorMap[level]) +
                    " (".Pastel(MiscChar) +
                    string.Join('.', logger.Name).Pastel(LessImportantText) +
                    ")".Pastel(MiscChar) +
                    "]".Pastel(MiscChar) +
                    " " +
                    message.Pastel(logLevelColorMap[level]);
                Console.WriteLine(message);
            });
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
            Console.Write("\e[3J");
        }

        public static void ShowInfo(string message) {
            new InfoSimpleMenu(message).Show();
        }

        public static bool YesNoInput(string yesDescription, string noDescription, bool? defaultAnswer = null) {
            return YesNoInput($"Y - {yesDescription}\nN - {noDescription}", defaultAnswer);
        }

        /// <summary>
        ///     A yes/no input with optional default yes/no
        /// </summary>
        /// <returns>
        ///     Yes -> <c>true</c><br />
        ///     No -> <c>false</c>
        /// </returns>
        public static bool YesNoInput(string? prompt = null, bool? defaultAnswer = null, bool addNewlines = true) {
            Console.CursorVisible = false;
            Console.Write((prompt ?? "") +
                (addNewlines ? "\n\n" : "") +
                $" [{(defaultAnswer ?? false ? "Y" : "y")}/{(defaultAnswer ?? true ? "n" : "N")}]");

            return TryFinally(() => {
                while (true) {
                    var key = Console.ReadKey(true).Key;

                    if (key is ConsoleKey.Y or ConsoleKey.N) {
                        return key == ConsoleKey.Y;
                    }

                    if (defaultAnswer is not null && key == ConsoleKey.Enter) {
                        return (bool)defaultAnswer;
                    }
                }
            }, result => {
                Console.WriteLine(" " + (result ? 'y' : 'n'));
                Console.CursorVisible = true;
            });
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

        // Original code thanks to Tomas Petricek @ https://stackoverflow.com/a/2359452/10518428
        public static T TryFinally<T>(Func<T> body, Action<T> finallyHandler) {
            var result = default(T);
            try {
                result = body();
            } finally {
                finallyHandler(result!);
            }

            return result;
        }

        /// <returns>The type of the caller's class that invoked the method where this helper is used.</returns>
        public static Type GetCallerClassType() {
            var constructor = new StackTrace().GetFrame(2)!.GetMethod()!;
            var classType = constructor.DeclaringType!;
            return classType;
        }

        public static IItemDataSource? CreateItemDataSource(GameInv gameInv) {
            return (MyEnv.GetString("STORAGE_TYPE") ?? "").ToLower() switch {
                "mysql" => new MySqlItemDataSource(MyEnv.GetString("DB_CONNECTION_STRING") ?? ""),
                "json" => new JsonItemDataSource(gameInv),
                _ => null
            };
        }

        public static void CheckDbConnectionString(IErrorPresenter errorPresenter) {
            var useDb = MyEnv.GetBool("USE_DB") ?? false;
            // ReSharper disable once InvertIf
            if (useDb && string.IsNullOrEmpty(MyEnv.GetString("DB_CONNECTION_STRING"))) {
                errorPresenter.Present(string.Format(Errors.NoDbConnectionString, EnvPrefix));
                Environment.Exit(1);
            }
        }

        public static string FormatException(Exception exception) {
            return $"Error: {exception.GetType().FullName}: {exception.Message}";
        }

        public class RefreshableObservableCollection<T> : ObservableCollection<T> {
            public void RefreshItem(T item) {
                var index = IndexOf(item);
                if (index < 0) return;
                RemoveAt(index);
                Insert(index, item);
            }
        }

        public class ResettableCountdownTimer(Action callback, int interval) {
            private CancellationTokenSource? _cts;

            public void Reset() {
                _cts?.Cancel();
                _cts = new();
                var cancellationToken = _cts.Token;

                // ReSharper disable once MethodSupportsCancellation
                Task.Run(async () => {
                    try {
                        await Task.Delay(interval, cancellationToken);
                        callback();
                    } catch (TaskCanceledException) { }
                });
            }
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
                [typeof(string), typeof(T).MakeByRefType()], null);
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
