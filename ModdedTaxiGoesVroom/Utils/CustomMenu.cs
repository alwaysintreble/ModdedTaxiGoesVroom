using System;
using System.Collections.Generic;
using System.Reflection;
using ModdedTaxiGoesVroom.Managers;

namespace ModdedTaxiGoesVroom.Utils;

public class CustomMenu(string title, int startIndex = 0)
{
    private int _origMenuIndex;
    public int OrigVoiceIndex;
    private CustomMenu _lastMenu;
    private readonly List<MenuButton> _buttons = [];
    public readonly int StartingIndex = startIndex;

    /// <summary>
    /// Adds a new button to the menu.
    /// </summary>
    /// <param name="button"></param>
    public void AddButton(MenuButton button)
    {
        Plugin.BepinLogger.LogDebug($"Registering button {button.GetText()}");
        _buttons.Add(button);
    }

    /// <summary>
    /// Hooks this menu to be the currently loaded menu.
    /// </summary>
    public void LoadMenu(MenuV2Script instance)
    {
        Plugin.BepinLogger.LogDebug($"loading menu {ToString()}");
        _lastMenu = MenuManager.CurrentMenu;
        MenuManager.CurrentMenu = this;
        _origMenuIndex = instance.menuIndex;
        OrigVoiceIndex = instance.voiceIndex;
        instance.voiceIndex = StartingIndex;
        RefreshMenu();
    }

    /// <summary>
    /// Returns to the previous menu.
    /// </summary>
    public void GoBack(MenuV2Script instance)
    {
        try
        {
            MenuManager.CurrentMenu = _lastMenu;
            foreach (var button in _buttons)
            {
                button.CurrentIndex = -1;
            }

            instance.menuIndex = _origMenuIndex;
            instance.voiceIndex = OrigVoiceIndex;
            RefreshMenu();
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
        }
    }

    public CustomMenu RemoveMenu(MenuV2Script instance)
    {
        GoBack(instance);
        return _lastMenu;
    }

    /// <summary>
    /// Clears this and all previous custom menus, resetting back to the base menu.
    /// </summary>
    protected void ClearMenus(MenuV2Script instance = null)
    {
        if (instance == null) instance = MenuV2Script.instance;
        var previousMenu = RemoveMenu(instance);
        while (previousMenu != null)
        {
            previousMenu = previousMenu.RemoveMenu(instance);
        }
    }

    /// <summary>
    /// registers the button text
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public string[] GetMenuStrings()
    {
        try
        {
            Plugin.BepinLogger.LogDebug($"Getting strings for {ToString()}");
            List<string> list = new List<string>();
            foreach (var button in _buttons)
            {
                var text = button.GetText();
                Plugin.BepinLogger.LogDebug(text);
                if (button.IsEnabled()) list.Add(text);
            }

            return list.ToArray();
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
            return [$"{this}"];
        }
    }

    /// <summary>
    /// execute the custom button's action
    /// </summary>
    /// <param name="instance"></param>
    public void SelectButton(MenuV2Script instance)
    {
        Sound.Play_Unpausable("SoundMenuSelect");
        var curIndex = 0;
        foreach (var button in _buttons)
        {
            if (button.IsEnabled())
            {
                if (instance.voiceIndex == curIndex)
                {
                    Plugin.BepinLogger.LogDebug($"pressed {button.GetText()}");
                    button.OnClick(instance);
                }

                curIndex++;
            }
        }

        RefreshMenu();
    }

    private void RefreshMenu()
    {
        MenuV2Script.instance.InvokeMethod("MenuVoicesInit");
    }

    public override string ToString()
    {
        return title;
    }
}