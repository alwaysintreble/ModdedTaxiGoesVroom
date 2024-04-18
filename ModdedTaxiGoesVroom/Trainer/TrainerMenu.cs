using ModdedTaxiGoesVroom.Managers;
using ModdedTaxiGoesVroom.Utils;

namespace ModdedTaxiGoesVroom.Trainer;

public class TrainerMenu : CustomMenu
{
    private bool allLevelsUnlocked;

    public TrainerMenu() : base("Trainer")
    {
        var teleportMenu = new TeleportMenu();
        AddButton(new MenuButton("Teleport", teleportMenu.LoadMenu));
        allLevelsUnlocked = true;
        foreach (var data in Data.levelDataList)
        {
            if (data.levelUnlocked) continue;
            allLevelsUnlocked = false;
            break;
        }
        AddButton(new MenuButton(() => allLevelsUnlocked ? "Lock All Levels" : "Unlock All Levels", ToggleLockedLevels));
        AddButton(new MenuButton(() => Master.instance.DEBUG ? "Disable Debug" : "Enable Debug", ToggleDebug));
        AddButton(new MenuButton(
            () => Master.instance.SHOW_TESTER_BUTTONS ? "Disable Inputs Overlay" : "Enable Inputs Overlay",
            ToggleTesterButtons));
        var playerManager = PlayerManager.Instance;
        AddButton(new MenuButton(playerManager.GetDeathTrainerText, playerManager.ChangeDeathBehavior));
        AddButton(new MenuButton(LocalizationHelper.GoBackText, GoBack));
    }

    private void ToggleLockedLevels()
    {
        allLevelsUnlocked = !allLevelsUnlocked;
        foreach (var data in Data.levelDataList)
        {
            data.levelUnlocked = allLevelsUnlocked;
        }
    }

    private void ToggleDebug()
    {
        Master.instance.DEBUG = !Master.instance.DEBUG;
    }

    private void ToggleTesterButtons()
    {
        Master.instance.SHOW_TESTER_BUTTONS = !Master.instance.SHOW_TESTER_BUTTONS;
    }
}