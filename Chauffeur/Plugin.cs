using System;
using System.IO;
using System.Linq;
using BepInEx;
using Chauffeur.Managers;
using Chauffeur.Menus;
using Chauffeur.Utils;
using Chauffeur.Utils.MenuUtils;

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
        ChauffeurLog.ChauffeurLogger = Logger;
        var config = new ChauffeurConfiguration();
        try
        {
            _logDirectory = Path.Combine(Path.GetDirectoryName(Paths.ExecutablePath), "ChauffeurLogs");
            ChauffeurLog.ChauffeurLogger.LogDebug(_logDirectory);
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
            ChauffeurLog.ChauffeurLogger.LogError(e);
        }

        ChauffeurLog.ChauffeurLogger.LogMessage($"{ModDisplayInfo} loaded!");
        On.ModMaster.Start += (orig, self) =>
        {
            self.ModEnableSet(true);
            orig(self);
            var menuManager = new MenuManager();
        };
    }
}