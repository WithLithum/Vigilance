using Rage;

namespace Vigilance.Engine
{
    internal static class Log
    {
        private static void LogOutput(string sender, string lvl, string message)
        {
            Game.LogTrivial($"[{sender}/{lvl}] {message}");
        }

        private static void LogVerbose(string sender, string lvl, string message)
        {
            Game.LogVerbose($"[{sender}/{lvl}] {message}");
        }

        internal static void Trace(string sender, string message)
        {
            LogOutput(sender, "TRACE", message);
        }

        internal static void TraceVerbose(string sender, string message)
        {
            LogVerbose(sender, "TRACE", message);
        }

        internal static void Info(string sender, string message)
        {
            LogOutput(sender, "INFO", message);
        }

        internal static void Warning(string sender, string message)
        {
            LogOutput(sender, "WARN", message);
        }

        internal static void Error(string sender, string message)
        {
            LogOutput(sender, "ERROR", message);
        }

        internal static void Fatal(string sender, string message)
        {
            LogOutput(sender, "FATAL", message);
        }
    }
}
