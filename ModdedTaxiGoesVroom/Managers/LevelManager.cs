using ModdedTaxiGoesVroom.Trainer;
using ModdedTaxiGoesVroom.Utils;

namespace ModdedTaxiGoesVroom.Managers;

public class LevelManager
{
    public static LevelManager Instance;

    public LevelManager()
    {
        On.ModMaster.OnLevelStart += LevelStart;
        Instance = this;
    }

    private void LevelStart(On.ModMaster.orig_OnLevelStart orig, ModMaster self)
    {
        Plugin.BepinLogger.LogDebug("hi :)");
        PlayerManager.Instance.GetPlayerAttrs();
        orig(self);
    }
}