using ModdedTaxiGoesVroom.Utils;

namespace ModdedTaxiGoesVroom.Trainer;

public class TrainerMenu : CustomMenu
{
    private bool allLevelsUnlocked;

    public TrainerMenu() : base("Trainer")
    {
        var teleportMenu = new TeleportMenu();
        AddButton(new MenuButton("Teleport", teleportMenu.LoadMenu));
        AddButton(new MenuButton("Unlock All Levels", UnlockLevels, () => !allLevelsUnlocked));
        allLevelsUnlocked = true;
        foreach (var data in Data.levelDataList)
        {
            if (data.levelUnlocked) continue;
            allLevelsUnlocked = false;
            break;
        }
        AddButton(new MenuButton(LocalizationHelper.GoBackText, GoBack));
    }

    private void UnlockLevels()
    {
        foreach (var data in Data.levelDataList)
        {
            data.levelUnlocked = true;
        }

        allLevelsUnlocked = !allLevelsUnlocked;
    }
}