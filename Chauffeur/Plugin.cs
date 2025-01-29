using System;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using Chauffeur.Managers;
using Chauffeur.Utils;

namespace Chauffeur;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "com.alwaysintreble.Chauffeur";
    public const string PluginName = "Chauffeur";
    public const string PluginVersion = "0.1.0";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    public static ManualLogSource ChauffeurLogger;
    private string _logDirectory;

    private void Awake()
    {
        ChauffeurLogger = Logger;
        var config = new ChauffeurConfiguration();
        try
        {
            _logDirectory = Path.Combine(Path.GetDirectoryName(Paths.ExecutablePath), "ChauffeurLogs");
            ChauffeurLogger.LogDebug(_logDirectory);
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

            var logPath = Path.Combine(_logDirectory, $"Chauffeur{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.log");
            BepInEx.Logging.Logger.Listeners.Add(new ChauffeurLog(logPath,
                config.LogLevel));
        }
        catch (Exception e)
        {
            ChauffeurLogger.LogError(e);
        }

        ChauffeurLogger.LogMessage($"{ModDisplayInfo} loaded!");
        On.ModMaster.Start += (orig, self) =>
        {
            self.ModEnableSet(true);
            orig(self);
            var menuManager = new MenuManager();
        };
    }
}