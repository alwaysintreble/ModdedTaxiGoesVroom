using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace Chauffer.Utils;

public class ChaufferConfiguration
{
    private ConfigEntry<LogLevel> _logLevel;
    private ConfigEntry<int> _logHistory;

    public LogLevel LogLevel => _logLevel.Value;
    public int LogHistory => _logHistory.Value;

    public ChaufferConfiguration()
    {
        var configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "Chauffer.cfg"), true);
        _logLevel = configFile.Bind<LogLevel>("Logging", "LogLevel",
            LogLevel.Fatal | LogLevel.Error | LogLevel.Warning | LogLevel.Message | LogLevel.Info,
            "Which log levels are output to the ChaufferLog.");
        _logHistory = configFile.Bind("Logging", "LogHistory", 5, "How many logs to keep a history of.");
    }
}