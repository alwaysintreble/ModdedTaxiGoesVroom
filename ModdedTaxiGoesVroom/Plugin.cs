using BepInEx;
using BepInEx.Logging;
using Chauffer.Managers;
using Chauffer.Utils;
using ModdedTaxiGoesVroom.Managers;
using ModdedTaxiGoesVroom.Trainer;

namespace ModdedTaxiGoesVroom;

[BepInDependency("com.alwaysintreble.Chauffer")]
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "com.alwaysintreble.ModdedTaxi";
    public const string PluginName = "ModdedTaxiGoesVroom";
    public const string PluginVersion = "0.1.0";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    public static ManualLogSource BepinLogger;
    public static Plugin Instance;

    private void Awake()
    {
        // Plugin startup logic
        BepinLogger = Logger;
        Instance = this;

        BepinLogger.LogMessage($"{ModDisplayInfo} loaded!");
        MenuManager.AddButtons += () =>
        {
            var assetManager = new AssetManager();
            var trainerMenu = new TrainerMenu();
            assetManager.CreateAssetSwapMenus();
            MenuManager.AddPauseMenuButton(new MenuButton(() => "Trainer", trainerMenu.LoadMenu, () => true));
        };
    }
}