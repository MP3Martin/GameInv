using DotNetEnv;

namespace GameInv.UtilsNS {
    public static partial class Utils {
        public static class MyEnv {
            public static void LoadEnv() {
                var options = Env.NoEnvVars().TraversePath().Load();
                foreach (var (key, value) in options) {
                    Environment.SetEnvironmentVariable(EnvPrefix + key, value);
                }
            }

            private static string PrefixKey(string key) {
                return EnvPrefix + key;
            }

            public static string? GetString(string key, string? fallback = null) {
                return Environment.GetEnvironmentVariable(PrefixKey(key)) ?? fallback;
            }

            public static bool? GetBool(string key, bool? fallback = null) {
                return !bool.TryParse(Environment.GetEnvironmentVariable(PrefixKey(key)), out var result) ? fallback : result;
            }

            public static int? GetInt(string key, int? fallback = null) {
                return !int.TryParse(Environment.GetEnvironmentVariable(PrefixKey(key)), out var result) ? fallback : result;
            }
        }
    }
}
