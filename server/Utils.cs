using System.Diagnostics;
using System.Reflection;

namespace GameInv {
    public static class Utils {
        /// <summary>
        ///     Do not use this outside of a class constructor <br />
        ///     Basically do not use this unless you are absolutely sure you know what you are doing
        /// </summary>
        public static Logger GetLogger() {
            // Gets the constructor (because GetLogger (this method) gets called in it)
            var constructor = new StackTrace().GetFrame(1)!.GetMethod()!;

            // Gets the class that contains the constructor
            var classType = constructor.DeclaringType!;

            return Logger.GetLogger(classType);
        }
    }
}
