using ModdedTaxiGoesVroom.Managers;
using ModdedTaxiGoesVroom.Utils;

namespace ModdedTaxiGoesVroom.Trainer;

public class TrainerMenu : CustomMenu
{
    private bool _allLevelsUnlocked;

    public TrainerMenu() : base("Trainer")
    {
        var teleportMenu = new TeleportMenu();
        AddButton(new MenuButton("Teleport", teleportMenu.LoadMenu));
        _allLevelsUnlocked = true;
        foreach (var data in Data.levelDataList)
        {
            if (data.levelUnlocked) continue;
            _allLevelsUnlocked = false;
            break;
        }
        AddButton(new MenuButton(() => _allLevelsUnlocked ? "Lock All Levels" : "Unlock All Levels", ToggleLockedLevels));
        AddButton(new MenuButton(() => Master.instance.DEBUG ? "Disable Debug" : "Enable Debug",
            () => Master.instance.DEBUG = !Master.instance.DEBUG));
        AddButton(new MenuButton(
            () => Master.instance.SHOW_TESTER_BUTTONS ? "Disable Inputs Overlay" : "Enable Inputs Overlay",
            () => Master.instance.SHOW_TESTER_BUTTONS = !Master.instance.SHOW_TESTER_BUTTONS));
        
        AddPlayerManagementButtons();
        AddLevelManagementButtons();
    }

    private void AddPlayerManagementButtons()
    {
        var playerManager = new PlayerManager();
        AddButton(new MenuButton(() => playerManager.CanBoost ? "Disable FOW Boosting" : "Enable FOW Boosting",
            () => playerManager.CanBoost = !playerManager.CanBoost));
        AddButton(new MenuButton(() => playerManager.CanFlip ? "Disable FOW Cancel" : "Enable FOW Cancel",
            () => playerManager.CanFlip = !playerManager.CanFlip));
        // AddButton(new MenuButton(playerManager.CanBackFlip ? "Disable Backflip" : "Enable Backflip",
        //     () => playerManager.CanBackFlip = !playerManager.CanBackFlip));
        AddButton(new MenuButton(() => playerManager.CanBounce ? "Disable Bouncing": "Enable Bouncing",
            () => playerManager.CanBounce = !playerManager.CanBounce));
        AddButton(new MenuButton(() => playerManager.CanDoubleBoost ? "Disable Double Boosting" : "Enable Double Boosting",
            () => playerManager.CanDoubleBoost = !playerManager.CanDoubleBoost));
    }

    private void AddLevelManagementButtons()
    {
        var levelManager = new LevelManager();
        AddButton(new MenuButton(levelManager.GetDeathTrainerText, levelManager.ChangeDeathBehavior));
        AddButton(new MenuButton(LocalizationHelper.GoBackText, GoBack));
    }

    private void ToggleLockedLevels()
    {
        _allLevelsUnlocked = !_allLevelsUnlocked;
        foreach (var data in Data.levelDataList)
        {
            data.levelUnlocked = _allLevelsUnlocked;
        }
    }
}