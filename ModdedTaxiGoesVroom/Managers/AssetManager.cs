using System;
using System.Collections.Generic;
using System.IO;
using ModdedTaxiGoesVroom.AssetSwapping;
using ModdedTaxiGoesVroom.Utils;
using UnityEngine;

namespace ModdedTaxiGoesVroom.Managers;

public class AssetManager
{
    public static AssetManager Instance;
    private Dictionary<string, byte[]> _taxiSkin;
    public static string TaxiSkinsPath => Directory.GetCurrentDirectory() + "\\TaxiSkins";
    public bool SkinLoaded = false;

    public AssetManager()
    {
        Instance = this;
        // On.AssetMaster.GetTexture2D += GetTexture2D;
        // On.AssetMaster.GetPrefab += GetPrefab;
        // On.AssetMaster.GetSprite += GetSprite;
        // On.AssetMaster.Awake += MasterAwake;
        On.PlayerScript.TaxiTextureGlassGet += TaxiTextureGlassGet;
    }

    private Texture[] TaxiTextureGlassGet(On.PlayerScript.orig_TaxiTextureGlassGet orig, PlayerScript self)
    {
        if (SkinLoaded || _taxiSkin == null) return orig(self);
        var textures = orig(self);
        foreach (var texture in textures)
        {
            if (!_taxiSkin.TryGetValue(texture.name, out var fileData)) continue;
            ((Texture2D)texture).LoadImage(fileData);
        }

        SkinLoaded = true;

        return textures;
    }

    private void LoadImage(string path)
    {
        var fileData = File.ReadAllBytes(path);
        var names = path.Split('\\');
        var name = names[names.Length - 1].TrimEnd('.', 'p', 'n', 'g', 'j', 'e');
        _taxiSkin[name] = fileData;
    }

    public void LoadSkin(string path)
    {
        _taxiSkin = new Dictionary<string, byte[]>();
        foreach (var file in Directory.EnumerateFiles(path))
        {
            if (!file.EndsWith(".png") && !file.EndsWith(".jpg") && !file.EndsWith(".jpeg"))
            {
                throw new ArgumentException($"{file} is invalid. Must be png or jpg");
            }
            LoadImage(file);
        }
    }

    private void MasterAwake(On.AssetMaster.orig_Awake orig, AssetMaster self)
    {
        orig(self);
        Plugin.BepinLogger.LogDebug("sounds");
        foreach (var sound in self.soundsDict.Keys)
        {
            Plugin.BepinLogger.LogDebug(sound);
        }

        Plugin.BepinLogger.LogDebug("music");
        foreach (var song in self.ostsDict.Keys)
        {
            Plugin.BepinLogger.LogDebug(song);
        }

        Plugin.BepinLogger.LogDebug("prefabs");
        foreach (var prefab in self.prefabsDict.Keys)
        {
            Plugin.BepinLogger.LogDebug(prefab);
        }

        Plugin.BepinLogger.LogDebug("textures");
        foreach (var texture in self.textures2DDict.Keys)
        {
            Plugin.BepinLogger.LogDebug(texture);
        }

        Plugin.BepinLogger.LogDebug("sprites");
        foreach (var sprite in self.spritesDict.Keys)
        {
            Plugin.BepinLogger.LogDebug(sprite);
        }
    }

    private Sprite GetSprite(On.AssetMaster.orig_GetSprite orig, string spritename)
    {
        if (spritename.Contains("TaxiGlass"))
        {
            Plugin.BepinLogger.LogDebug($"Getting a taxi sprite! {spritename}");
        }

        return orig(spritename);
    }

    private Texture2D GetTexture2D(On.AssetMaster.orig_GetTexture2D orig, string texturename)
    {
        if (texturename.Contains("TaxiGlass"))
        {
            Plugin.BepinLogger.LogDebug($"Getting a taxi texture! {texturename}");
        }

        return orig(texturename);
    }

    private GameObject GetPrefab(On.AssetMaster.orig_GetPrefab orig, string prefabname)
    {
        if (prefabname.Contains("TaxiGlass"))
        {
            Plugin.BepinLogger.LogDebug($"Getting a taxi prefab! {prefabname}");
        }

        return orig(prefabname);
    }

    public void CreateAssetSwapMenus()
    {
        try
        {
            var taxiSkinMenu = new TaxiSkinMenu();
            MenuManager.Instance.AddMainMenuButton(new MenuButton("Change Taxi Skin", taxiSkinMenu.LoadMenu));
        }
        catch (Exception e)
        {
            if (e is DirectoryNotFoundException) return;
            Plugin.BepinLogger.LogError(e);
        }
    }

    public void ClearCustomSkin()
    {
        _taxiSkin = null;
    }
}