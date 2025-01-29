using System;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using Chauffer.Managers;
using Chauffer.Utils;

namespace Chauffer;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "com.alwaysintreble.Chauffer";
    public const string PluginName = "Chauffer";
    public const string PluginVersion = "0.1.0";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    public static ManualLogSource ChaufferLogger;
    private string _logDirectory;

    private void Awake()
    {
        ChaufferLogger = Logger;
        var config = new ChaufferConfiguration();
        try
        {
            _logDirectory = Path.Combine(Path.GetDirectoryName(Paths.ExecutablePath), "ChaufferLogs");
            ChaufferLogger.LogDebug(_logDirectory);
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            var currentLogs = new DirectoryInfo(_logDirectory).GetFiles();
            if (currentLogs.Length >= config.LogHistory)
            {
                var oldestLog = currentLogs.OrderBy(file => file.CreationTime).FirstOrDefault();
                oldestLog?.Delete();
            }

            var logPath = Path.Combine(_logDirectory, $"Chauffer{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log");
            BepInEx.Logging.Logger.Listeners.Add(new ChaufferLog(logPath,
                config.LogLevel));
        }
        catch (Exception e)
        {
            ChaufferLogger.LogError(e);
        }

        ChaufferLogger.LogMessage($"{ModDisplayInfo} loaded!");
        On.ModMaster.Start += (orig, self) =>
        {
            self.ModEnableSet(true);
            orig(self);
            var menuManager = new MenuManager();
        };
    }
}