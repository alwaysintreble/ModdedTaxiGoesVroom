using System;
using System.Collections.Generic;
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
            var menuManager = new MenuManager();
            On.MenuV2Element.UpdateTexts += menuManager.UpdateTexts;
            On.MenuV2Script.MenuBack += menuManager.MenuBack;

            var testMainMenu = new CustomMenu("test");
            testMainMenu.AddButton(new MenuButton(() => "Go Back", testMainMenu.GoBack, () => true));
            var trainerMenu = new TrainerMenu();
            menuManager.AddMainMenuButton(new MenuButton(() => "Hello World!", testMainMenu.LoadMenu, () => true));
            testMainMenu.AddButton(new MenuButton("Text Prompt",
                () => MenuHelpers.TextInput(
                    "Test",
                    text => BepinLogger.LogDebug(text),
                    "This is a test prompt",
                    "Hello World")));
            testMainMenu.AddButton(new MenuButton("Ask Prompt",
                () => MenuHelpers.AskYesNo(
                    "Test",
                    confirm => BepinLogger.LogDebug(confirm),
                    "Would you like a cookie?")));
            menuManager.AddPauseMenuButton(new MenuButton(() => "Trainer", trainerMenu.LoadMenu, () => true));
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