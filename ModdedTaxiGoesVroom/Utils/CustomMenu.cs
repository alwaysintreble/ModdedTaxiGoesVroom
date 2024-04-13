using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RandoTaxiGoesVroom.Managers;

namespace RandoTaxiGoesVroom.Utils;

public class CustomMenu(CustomMenu.MenuType type, int startIndex = 0)
{
    private static MethodInfo _menuInit =
        typeof(MenuV2Script).GetMethod("MenuVoicesInit", BindingFlags.NonPublic | BindingFlags.Instance);
    public enum MenuType
    {
        MainMenu,
        PauseMenu
    }

    public readonly MenuType Type = type;
    private int startingIndex = startIndex;
    private readonly List<MenuButton> buttons = [];

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
    public void LoadMenu()
    {
        Plugin.BepinLogger.LogMessage($"loading menu {ToString()}");
        MenuButtonManager.CurrentMenu = this;
        _menuInit.Invoke(MenuV2Script.instance, []);
        MenuV2Script.instance.menuIndex = startingIndex;
    }

    /// <summary>
    /// Removes this menu to return to either the regular or another menu.
    /// </summary>
    public void RemoveMenu()
    {
        MenuButtonManager.CurrentMenu = null;
        foreach (var button in buttons)
        {
            button.CurrentIndex = -1;
        }
        _menuInit.Invoke(MenuV2Script.instance, []);
    }

    /// <summary>
    /// registers the button text
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public string[] GetMenuStrings(MenuV2Script self)
    {
        Plugin.BepinLogger.LogMessage($"Getting strings for {ToString()}");
        return (from button in buttons where button.IsEnabled() select button.GetText()).ToArray();
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
                    button.OnClick();
                }

                curIndex++;
            }
        }
    }
}