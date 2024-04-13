using System;
using BepInEx;
using BepInEx.Logging;
using MonoMod.Utils;
using RandoTaxiGoesVroom.Managers;
using RandoTaxiGoesVroom.Utils;
using UnityEngine;

namespace RandoTaxiGoesVroom;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGUID = "com.alwaysintreble.ModdedTaxi";
    public const string PluginName = "Logger";
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
        };
        // On.ModMaster.OnPlayerDie += OnPlayerDie;
        // On.CarSpawnerScript.Start += CarSpawnStart;
        // GUI.enabled = true;
        // On.Master.Update += (orig, self) =>
        // {
        //     OnGUI();
        //     orig(self);
        // };
        // On.MenuV2Element.UpdateTexts += UpdateTexts;
        var buttonManager = new MenuButtonManager();
        var archipelagoMenu = new CustomMenu(CustomMenu.MenuType.MainMenu);
        archipelagoMenu.AddButton(new MenuButton(() => "this", null, () => true));
        archipelagoMenu.AddButton(new MenuButton(() => "is", null, () => true));
        archipelagoMenu.AddButton(new MenuButton(() => "a", null, () => true));
        archipelagoMenu.AddButton(new MenuButton(() => "test", null, () => true));
        archipelagoMenu.AddButton(new MenuButton(() => "Go Back", archipelagoMenu.RemoveMenu, () => true));
        var trainerMenu = new CustomMenu(CustomMenu.MenuType.PauseMenu);
        trainerMenu.AddButton(new MenuButton(() => "test", trainerMenu.RemoveMenu, () => true));
        // archipelagoMenu.LoadMenu();
        buttonManager.AddMainMenuButton(new MenuButton(() => "Hello World!", archipelagoMenu.LoadMenu, () => true));
        buttonManager.AddPauseMenuButton(new MenuButton(() => "Trainer", trainerMenu.LoadMenu, () => true));
    }

    private void PauseTest()
    {
        BepinLogger.LogMessage("Hello World!");
    }

    void Test()
    {
        BepinLogger.LogMessage("Hello World!");
    }

    private void CarSpawnStart(On.CarSpawnerScript.orig_Start orig, CarSpawnerScript self)
    {
        BepinLogger.LogMessage("car spawn started");
        orig(self);
    }

    private void UpdateTexts(On.MenuV2Element.orig_UpdateTexts orig)
    {
        BepinLogger.LogMessage("update texts called");
        // orig();
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

    private void OnLevelWasLoaded(int level)
    {
        BepinLogger.LogMessage(level.ToString());
    }

    private void OnPlayerDie(On.GameplayMaster.orig_Die orig, GameplayMaster self)
    {
        orig(self);
        try
        {
            // ArchipelagoConsole.LogMessage("player died");
            // Level.Restart();
        }
        catch (Exception e)
        {
            e.LogDetailed();
        }
    }

    private void Update()
    {
        try
        {
            GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo);
        }
        catch (Exception e)
        {
            e.LogDetailed();
        }
    }
}