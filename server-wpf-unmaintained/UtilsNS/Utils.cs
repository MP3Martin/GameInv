using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using GameInv_WPF.ItemNS;
using Pastel;
using Sherlog;
using Color = System.Drawing.Color;

namespace GameInv_WPF.UtilsNS {
    public static partial class Utils {
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
                    string.Join('.', logger.Name.Split('.')[1..]).Pastel(LessImportantText) +
                    ")".Pastel(MiscChar) +
                    "]".Pastel(MiscChar) +
                    " " +
                    message.Pastel(logLevelColorMap[level]);
                Console.WriteLine(message);
            });
        }

        public static DependencyObject? FindChildByName(DependencyObject parent, string name) {
            // Iterate through all children of the parent
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is the correct type and name, return it
                if (child is FrameworkElement fe && fe.Name == name) {
                    return child;
                }

                // Recurse into the child if it's a container
                var result = FindChildByName(child, name);
                if (result is not null) {
                    return result;
                }
            }

            return null;
        }

        public static MessageBoxResult ShowErrorMessageBox(string message) {
            return MessageBox.Show(message, "Error");
        }

        public class RefreshableObservableCollection<T> : ObservableCollection<T> {
            public void RefreshItem(T item) {
                var index = IndexOf(item);
                if (index < 0) return;
                RemoveAt(index);
                Insert(index, item);
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
