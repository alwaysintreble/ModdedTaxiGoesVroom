using Chauffer.Utils;
using ModdedTaxiGoesVroom.Managers;

namespace ModdedTaxiGoesVroom.Trainer;

public class PlayerControlsMenu: CustomMenu
{
    public PlayerControlsMenu() : base("Change Controls")
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
}