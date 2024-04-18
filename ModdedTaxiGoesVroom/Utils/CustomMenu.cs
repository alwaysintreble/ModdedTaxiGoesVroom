using System;
using System.Collections.Generic;
using System.Reflection;
using ModdedTaxiGoesVroom.Managers;

namespace ModdedTaxiGoesVroom.Utils;

public class CustomMenu(string title, int startIndex = 0)
{
    private static readonly MethodInfo MenuInit =
        typeof(MenuV2Script).GetMethod("MenuVoicesInit", BindingFlags.NonPublic | BindingFlags.Instance);

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
        Plugin.BepinLogger.LogMessage($"Registering button {button.GetText()}");
        _buttons.Add(button);
    }

    /// <summary>
    /// Hooks this menu to be the currently loaded menu.
    /// </summary>
    public void LoadMenu(MenuV2Script instance)
    {
        Plugin.BepinLogger.LogMessage($"loading menu {ToString()}");
        _lastMenu = MenuButtonManager.CurrentMenu;
        MenuButtonManager.CurrentMenu = this;
        _origMenuIndex = instance.menuIndex;
        OrigVoiceIndex = instance.voiceIndex;
        instance.voiceIndex = StartingIndex;
        MenuInit.Invoke(instance, []);
    }

    /// <summary>
    /// Returns to the previous menu.
    /// </summary>
    public void GoBack(MenuV2Script instance)
    {
        try
        {
            MenuButtonManager.CurrentMenu = _lastMenu;
            foreach (var button in _buttons)
            {
                button.CurrentIndex = -1;
            }

            instance.menuIndex = _origMenuIndex;
            instance.voiceIndex = OrigVoiceIndex;
            MenuInit?.Invoke(instance, []);
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
    /// <param name="self"></param>
    public void SelectButton(MenuV2Script self)
    {
        Plugin.BepinLogger.LogMessage($"selected button in {ToString()}");
        Sound.Play_Unpausable("SoundMenuSelect");
        var curIndex = 0;
        foreach (var button in _buttons)
        {
            if (button.IsEnabled())
            {
                if (self.voiceIndex == curIndex)
                {
                    Plugin.BepinLogger.LogMessage($"pressed {button.GetText()}");
                    button.OnClick(self);
                }

                curIndex++;
            }
        }
    }

    public override string ToString()
    {
        return title;
    }
}