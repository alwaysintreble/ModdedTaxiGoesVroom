using System;
using System.Collections.Generic;
using System.IO;
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
    public static Plugin Instance;

    private void Awake()
    {
        // Plugin startup logic
        BepinLogger = Logger;
        Instance = this;

        BepinLogger.LogMessage($"{ModDisplayInfo} loaded!");
        On.ModMaster.Start += (orig, self) =>
        {
            self.ModEnableSet(true);
            orig(self);
            var assetManager = new AssetManager();
            var menuManager = new MenuManager();
            On.MenuV2Element.UpdateTexts += menuManager.UpdateTexts;
            On.MenuV2Script.MenuBack += menuManager.MenuBack;

            var trainerMenu = new TrainerMenu();
            assetManager.CreateAssetSwapMenus();
            menuManager.AddPauseMenuButton(new MenuButton(() => "Trainer", trainerMenu.LoadMenu, () => true));
        };
    }
}