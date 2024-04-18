using System;
using System.Collections.Generic;
using System.Linq;
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
        var currentLevel = GameplayMaster.instance.levelId;
        switch (_onDeath)
        {
            case DeathHandling.Nothing:
            default:
                break;
            case DeathHandling.ResetLevel:
                LoadLevel(IndexFromID((int)currentLevel));
                break;
            case DeathHandling.ResetLevelAndCollectibles:
                Plugin.BepinLogger.LogDebug("Reset Level and collectibles");
                try
                {
                    ResetCollectibles();
                }
                catch (Exception e)
                {
                    Plugin.BepinLogger.LogError(e);
                }
                LoadLevel(IndexFromID((int)currentLevel));
                break;
            case DeathHandling.ReturnToHub:
                PortalScript.GoToLevel(Levels.GetHubIndex(), Data.GetHubLevelId());
                break;
            case DeathHandling.ReturnToHubAndResetCollectibles:
                ResetCollectibles();
                break;
        }
        Data.SaveGame();
    }

    /// <summary>
    /// DebugMaster doesn't exist unless Master.Instance.DEBUG is true
    /// </summary>
    private void ResetCollectibles()
    {
        Plugin.BepinLogger.LogDebug(GameplayMaster.instance.levelId);
        foreach (var levelData in Data.levelDataList)
        {
            var dataID = (Data.LevelId)levelData.levelId;
            Plugin.BepinLogger.LogDebug(dataID);
            if (GameplayMaster.instance.levelId != dataID) continue;
            var levelGears = Data.GearsLevelCollectedNumber(levelData.levelId);
            if (!new []{11, 17, 18, 19}.Contains(levelData.levelId))
            {
                for (var gearIndex = 0; gearIndex < Master.GetLevelGearsMaxNumber(dataID); ++gearIndex)
                {
                    Data.GearStateSetAbsolute(levelData.levelId, gearIndex, false);
                }
            }

            Data.gearsUnlockedNumber[Data.gameDataIndex] -= levelGears;
            levelData.bunniesUnlocked = 0;
            levelData.coinsPickedUpState = new int[levelData.coinsPickedUpState.Length];
            levelData.peoplePickedUpState = new int[levelData.peoplePickedUpState.Length];
            
            Data.ComputeMaximumGameGears();
            BonusScript.BonusesRefreshMaterials();
            return;
        }
    }

    private void LoadLevel(int index)
    {
        CheckpointScript.CheckpointDataReset();
        GameplayMaster.SelfRespawnClear();
        Level.GoTo(index, true);
    }

    private int IndexFromID(int levelID)
    {
        var lookup = new Dictionary<int, Levels.Index>
        {
            { 0, Levels.Index.level_hub },
            { 1, Levels.Index.level_bombeach },
            { 2, Levels.Index.level_PizzaTime },
            { 3, Levels.Index.level_MoriosHome },
            { 4, Levels.Index.level_PanikArcade },
            { 5, Levels.Index.level_ToslaOffices },
            { 6, Levels.Index.level_Gym },
            { 7, Levels.Index.level_PoopWorld },
            { 8, Levels.Index.level_Sewers },
            { 9, Levels.Index.level_City },
            { 10, Levels.Index.level_CrashTestIndustries },
            { 11, Levels.Index.level_HubDEMO },
            { 12, Levels.Index.level_MoriosMind },
            { 13, Levels.Index.level_StarmanCastle },
            { 14, Levels.Index.level_ToslaHq },
            { 15, Levels.Index.level_Moon },
            { 16, Levels.Index.level_Rocket },
            { 17, Levels.Index.level_time_attack_01 },
            { 18, Levels.Index.level_time_attack_02 },
            { 19, Levels.Index.level_time_attack_03 }
        };
        return (int)lookup[levelID];
    }
}