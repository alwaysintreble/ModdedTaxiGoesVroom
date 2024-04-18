using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace ModdedTaxiGoesVroom.Managers;

public class PlayerManager
{
    public static PlayerManager Instance;
    private DeathHandling _onDeath = DeathHandling.Nothing;
    private Rigidbody _playerBody;
    private Transform _playerTransform;
    private PlayerScript _playerScript;
    public bool CanBoost = true;
    public bool CanFlip = true;
    public bool CanBackFlip = true;
    public bool CanDoubleBoost = true;
    public bool CanBounce = true;
    private bool _stopPlayer = false;

    enum DeathHandling
    {
        Nothing,
        ResetLevel,
        ResetLevelAndCollectibles,
        ReturnToHub,
        ReturnToHubAndResetCollectibles
    }

    public PlayerManager()
    {
        On.ModMaster.OnPlayerDie += OnPlayerDie;
        On.ModMaster.OnPlayerFlipOWill += OnFlip;
        On.ModMaster.OnPlayerIsBoosted += OnBoost;
        On.ModMaster.OnPlayerDoubleBoost += OnDoubleBoost;
        On.ModMaster.OnPlayerFlipOWill_Interrupt += OnFlipInterrupt;
        On.ModMaster.OnPlayerBounce_Jump += OnBounceJump;
        On.PlayerScript.Start += OnPlayerStart;
        Instance = this;
    }

    private void OnPlayerStart(On.PlayerScript.orig_Start orig, PlayerScript self)
    {
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

    public void GetPlayerAttrs()
    {
        if (ModMaster.instance == null || _playerBody != null) return;
        _playerBody = ModMaster.instance.PlayerGetRigidbody();
        _playerScript = ModMaster.instance.PlayerGetInstance();
        _playerTransform = ModMaster.instance.PlayerGetTransform();
    }

    private void OnFlip(On.ModMaster.orig_OnPlayerFlipOWill orig, ModMaster self)
    {
        orig(self);
        if (!CanBoost)
            ThreadPool.QueueUserWorkItem(_ => DelayedStopPlayer());
    }

    private void StopPlayer()
    {
        _playerScript.myRb.velocity = new Vector3(0, 0, 0);
    }

    private void DelayedStopPlayer()
    {
        Thread.Sleep(650);
        if (_stopPlayer)
            StopPlayer();
    }


    private void OnBoost(On.ModMaster.orig_OnPlayerIsBoosted orig, ModMaster self)
    {
        orig(self);
        if (!CanBoost)
            StopPlayer();
    }

    private void OnDoubleBoost(On.ModMaster.orig_OnPlayerDoubleBoost orig, ModMaster self)
    {
        orig(self);
        if (!CanDoubleBoost)
            StopPlayer();
    }

    private void OnFlipInterrupt(On.ModMaster.orig_OnPlayerFlipOWill_Interrupt orig, ModMaster self)
    {
        orig(self);
        if (!CanFlip)
            StopPlayer();
    }

    private void OnBounceJump(On.ModMaster.orig_OnPlayerBounce_Jump orig, ModMaster self)
    {
        orig(self);
        if (!CanBounce)
            StopPlayer();
    }
}