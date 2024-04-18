using System;
using BepInEx;
using BepInEx.Logging;
using ModdedTaxiGoesVroom.Managers;
using ModdedTaxiGoesVroom.Trainer;
using ModdedTaxiGoesVroom.Utils;
using MonoMod.Utils;

namespace ModdedTaxiGoesVroom;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "com.alwaysintreble.ModdedTaxi";
    public const string PluginName = "ModdedTaxiGoesVroom";
    public const string PluginVersion = "0.1.0";

    public const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
    public static ManualLogSource BepinLogger;

    private void Awake()
    {
        // Plugin startup logic
        BepinLogger = Logger;

        BepinLogger.LogMessage($"{ModDisplayInfo} loaded!");
        On.ModMaster.Start += (orig, self) =>
        {
            self.ModEnableSet(true);
            orig(self);
            var buttonManager = new MenuButtonManager();
            On.MenuV2Element.UpdateTexts += buttonManager.UpdateTexts;
            On.MenuV2Script.MenuBack += buttonManager.MenuBack;
            var levelManager = new LevelManager();
            var playerManager = new PlayerManager();

            var testMainMenu = new CustomMenu("test");
            testMainMenu.AddButton(new MenuButton(() => "Go Back", testMainMenu.GoBack, () => true));
            var trainerMenu = new TrainerMenu();
            buttonManager.AddMainMenuButton(new MenuButton(() => "Hello World!", testMainMenu.LoadMenu, () => true));
            buttonManager.AddPauseMenuButton(new MenuButton(() => "Trainer", trainerMenu.LoadMenu, () => true));
        };
    }

    private void OnPlayerDie(On.ModMaster.orig_OnPlayerDie orig, ModMaster self)
    {
        try
        {
            BepinLogger.LogMessage("player died");
            // Level.Restart();
        }
        catch (Exception e)
        {
            e.LogDetailed();
        }

        orig(self);
        PortalScript.GoToLevel(Levels.GetHubIndex(), Data.GetHubLevelId());
    }
}