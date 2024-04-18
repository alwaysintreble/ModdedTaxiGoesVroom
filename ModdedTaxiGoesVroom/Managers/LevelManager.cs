using System.Text.RegularExpressions;
using ModdedTaxiGoesVroom.Trainer;
using ModdedTaxiGoesVroom.Utils;

namespace ModdedTaxiGoesVroom.Managers;

public class LevelManager
{
    public static LevelManager Instance;
    private DeathHandling _onDeath = DeathHandling.Nothing;

    enum DeathHandling
    {
        Nothing,
        ResetLevel,
        ResetLevelAndCollectibles,
        ReturnToHub,
        ReturnToHubAndResetCollectibles
    }

    public LevelManager()
    {
        On.ModMaster.OnLevelStart += LevelStart;
        On.ModMaster.OnPlayerDie += OnPlayerDie;
        Instance = this;
    }

    private void LevelStart(On.ModMaster.orig_OnLevelStart orig, ModMaster self)
    {
        Plugin.BepinLogger.LogDebug("hi :)");
        PlayerManager.Instance.GetPlayerAttrs();
        orig(self);
    }

    public string GetDeathTrainerText()
    {
        return $"On Death: {string.Join(" ",Regex.Split(_onDeath.ToString(), "(?<!^)(?=[A-Z])"))}";
    }
    
    public void ChangeDeathBehavior()
    {
        _onDeath = _onDeath switch
        {
            DeathHandling.Nothing => DeathHandling.ResetLevel,
            DeathHandling.ResetLevel => DeathHandling.ResetLevelAndCollectibles,
            DeathHandling.ResetLevelAndCollectibles => DeathHandling.ReturnToHub,
            DeathHandling.ReturnToHub => DeathHandling.ReturnToHubAndResetCollectibles,
            _ => DeathHandling.Nothing
        };
    }

    private void OnPlayerDie(On.ModMaster.orig_OnPlayerDie orig, ModMaster self)
    {
        orig(self);
        switch (_onDeath)
        {
            case DeathHandling.ResetLevel:
                Level.Restart();
                break;
            case DeathHandling.Nothing:
            case DeathHandling.ResetLevelAndCollectibles:
            case DeathHandling.ReturnToHub:
            case DeathHandling.ReturnToHubAndResetCollectibles:
            default:
                break;
        }
        // PortalScript.GoToLevel(Levels.GetHubIndex(), Data.GetHubLevelId());
    }
}