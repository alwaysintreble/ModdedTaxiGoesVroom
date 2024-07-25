using BepInEx;
using BepInEx.Logging;
using Chauffer.Managers;

namespace Chauffer;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "com.alwaysintreble.Chauffer";
    public const string PluginName = "Chauffer";
    public const string PluginVersion = "0.1.0";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    public static ManualLogSource ChaufferLogger;
    public static Plugin Instance;

    private void Awake()
    {
        ChaufferLogger = Logger;
        Instance = this;
        ChaufferLogger.LogMessage($"{ModDisplayInfo} loaded!");
        On.ModMaster.Start += (orig, self) =>
        {
            self.ModEnableSet(true);
            orig(self);
            var menuManager = new MenuManager();
        };
    }
}
