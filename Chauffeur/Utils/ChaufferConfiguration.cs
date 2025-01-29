using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace Chauffeur.Utils;

public class ChauffeurConfiguration
{
    private ConfigEntry<LogLevel> _logLevel;
    private ConfigEntry<int> _logHistory;

    public LogLevel LogLevel => _logLevel.Value;
    public int LogHistory => _logHistory.Value;

    public ChauffeurConfiguration()
    {
        var configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "Chauffeur.cfg"), true);
        _logLevel = configFile.Bind<LogLevel>("Logging", "LogLevel",
            LogLevel.Fatal | LogLevel.Error | LogLevel.Warning | LogLevel.Message | LogLevel.Info,
            "Which log levels are output to the ChauffeurLog.");
        _logHistory = configFile.Bind("Logging", "LogHistory", 5, "How many logs to keep a history of.");
    }
}