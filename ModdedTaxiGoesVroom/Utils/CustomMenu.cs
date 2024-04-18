using System;
using System.Collections.Generic;
using System.Reflection;
using ModdedTaxiGoesVroom.Managers;

namespace ModdedTaxiGoesVroom.Utils;

public class CustomMenu
{
    private static readonly MethodInfo _menuInit =
        typeof(MenuV2Script).GetMethod("MenuVoicesInit", BindingFlags.NonPublic | BindingFlags.Instance);

    public enum MenuType
    {
        MainMenu,
        PauseMenu,
        Settings,
        PauseSettings,
        Nested
    }

    public readonly MenuType Type;
    private string title;
    private int startingIndex;
    public int origMenuIndex;
    private int origVoiceIndex;
    protected CustomMenu lastMenu;
    private readonly List<MenuButton> buttons = [];

    public CustomMenu(MenuType type, string title, int startIndex = 0)
    {
        Type = type;
        startingIndex = startIndex;
        this.title = title;
    }

    /// <summary>
    /// Adds a new button to the menu.
    /// </summary>
    /// <param name="button"></param>
    public void AddButton(MenuButton button)
    {
        Plugin.BepinLogger.LogMessage($"Registering button {button.GetText()}");
        buttons.Add(button);
    }

    /// <summary>
    /// Hooks this menu to be the currently loaded menu.
    /// </summary>
    public void LoadMenu(MenuV2Script instance)
    {
        Plugin.BepinLogger.LogMessage($"loading menu {ToString()}");
        lastMenu = MenuButtonManager.CurrentMenu;
        MenuButtonManager.CurrentMenu = this;
        _menuInit.Invoke(instance, []);
        origMenuIndex = instance.menuIndex;
        origVoiceIndex = instance.voiceIndex;
        instance.voiceIndex = startingIndex;
    }

    /// <summary>
    /// Returns to the previous menu.
    /// </summary>
    public void GoBack(MenuV2Script instance)
    {
        try
        {
            MenuButtonManager.CurrentMenu = lastMenu;
            foreach (var button in buttons)
            {
                button.CurrentIndex = -1;
            }

            instance.menuIndex = origMenuIndex;
            instance.voiceIndex = origVoiceIndex;
            _menuInit?.Invoke(instance, []);
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
        }
    }

    public CustomMenu RemoveMenu()
    {
        GoBack(MenuV2Script.instance);
        return lastMenu;
    }

    /// <summary>
    /// Clears this and all previous custom menus, resetting back to the base menu.
    /// </summary>
    public void ClearMenus()
    {
        var previousMenu = RemoveMenu();
        while (previousMenu != null)
        {
            previousMenu = previousMenu.RemoveMenu();
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
            Plugin.BepinLogger.LogMessage($"Getting strings for {ToString()}");
            List<string> list = new List<string>();
            foreach (var button in buttons)
            {
                if (button.IsEnabled()) list.Add(button.GetText());
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
        Plugin.BepinLogger.LogMessage($"selected a button in {ToString()}");
        Sound.Play_Unpausable("SoundMenuSelect");
        var curIndex = 0;
        foreach (var button in buttons)
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