using System.Threading;
using UnityEngine;

namespace ModdedTaxiGoesVroom.Managers;

public class PlayerManager
{
    public static PlayerManager Instance;
    private Rigidbody _playerBody;
    private Transform _playerTransform;
    private PlayerScript _playerScript;
    public bool CanBoost = true;
    public bool CanFlip = true;
    public bool CanBackFlip = true;
    public bool CanDoubleBoost = true;
    public bool CanBounce = true;
    private bool _stopPlayer;

    public PlayerManager()
    {
        On.ModMaster.OnPlayerFlipOWill += OnFlip;
        On.ModMaster.OnPlayerIsBoosted += OnBoost;
        On.ModMaster.OnPlayerDoubleBoost += OnDoubleBoost;
        On.ModMaster.OnPlayerFlipOWill_Interrupt += OnFlipInterrupt;
        On.ModMaster.OnPlayerBounce_Jump += OnBounceJump;
        On.PlayerScript.Start += OnPlayerStart;
        On.PlayerScript.Awake += OnPlayerAwake;
        Instance = this;
    }

    private void OnPlayerAwake(On.PlayerScript.orig_Awake orig, PlayerScript self)
    {
        AssetManager.Instance.SkinLoaded = false;
        orig(self);
    }

    private void OnPlayerStart(On.PlayerScript.orig_Start orig, PlayerScript self)
    {
        Plugin.BepinLogger.LogDebug("player started");
        orig(self);
    }

    public void GetPlayerAttrs()
    {
        if (PlayerScript.instance == null || _playerBody != null) return;
        _playerBody = ModMaster.instance.PlayerGetRigidbody();
        _playerScript = ModMaster.instance.PlayerGetInstance();
        _playerTransform = ModMaster.instance.PlayerGetTransform();
        if (_playerScript == null) return;
    }

    private void OnFlip(On.ModMaster.orig_OnPlayerFlipOWill orig, ModMaster self)
    {
        orig(self);
        if (CanBoost) return;
        _stopPlayer = true;
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
        
    }

    private void OnDoubleBoost(On.ModMaster.orig_OnPlayerDoubleBoost orig, ModMaster self)
    {
        orig(self);
        if (!CanDoubleBoost)
            StopPlayer();
        else
            _stopPlayer = false;
    }

    private void OnFlipInterrupt(On.ModMaster.orig_OnPlayerFlipOWill_Interrupt orig, ModMaster self)
    {
        orig(self);
        if (!CanFlip)
            StopPlayer();
        else
            _stopPlayer = false;
    }

    private void OnBounceJump(On.ModMaster.orig_OnPlayerBounce_Jump orig, ModMaster self)
    {
        orig(self);
        if (!CanFlip || !CanBounce)
            StopPlayer();
        else
            _stopPlayer = false;
    }
}