using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chauffer.Utils;
using Febucci.UI;

namespace Chauffer.Managers;

public class MenuManager
{
    private readonly List<MenuButton> _extraMainMenuButtons = [];
    private readonly List<MenuButton> _extraPauseMenuButtons = [];
    public static MenuManager Instance;
    public static CustomMenu CurrentMenu = null;

    public MenuManager()
    {
        On.MenuV2Script._PauseMenuDefineVoiceIndexes += PauseMenuDefineVoiceIndexes;
        On.MenuV2Script.PauseMenuVoicesStringsGet += GetPauseMenuStrings;
        On.MenuV2Script._SelectPauseMenu += SelectPauseMenuItem;
        On.MenuV2Script._MainMenuDefineVoiceIndexes += MainMenuDefineVoiceIndexes;
        On.MenuV2Script.MainMenuVoicesStringsGet += GetMainMenuStrings;
        On.MenuV2Script._SelectMainMenu += SelectMainMenuItem;
        On.MenuV2Element.UpdateTexts += UpdateTexts;
        On.MenuV2Script.MenuBack += MenuBack;
        Instance = this;
        AddButtons?.Invoke();
    }
    
    public static event AddButton AddButtons;
    public delegate void AddButton();

    private void MainMenuDefineVoiceIndexes(On.MenuV2Script.orig__MainMenuDefineVoiceIndexes orig, MenuV2Script self,
        MenuV2Script.MainMenuKind mkind, out int indexplay, out int indexwishlist, out int indexdiscord,
        out int indexextracredits, out int indexsett, out int indexquit, out int indexback, out int indexcheat)
    {
        orig(self, mkind, out indexplay, out indexwishlist, out indexdiscord, out indexextracredits, out indexsett,
            out indexquit, out indexback, out indexcheat);

        indexquit = self.MainMenuKindGet() switch
        {
            // 4 elements
            MenuV2Script.MainMenuKind.desktop_Normal or MenuV2Script.MainMenuKind.nonDesktop_Normal => 3 +
                                                                             _extraMainMenuButtons.Count(button => button.IsEnabled()),
            // 5 elements
            MenuV2Script.MainMenuKind.desktop_WithCheats or MenuV2Script.MainMenuKind.nonDesktop_WithCheats => 4 +
                _extraMainMenuButtons.Count(button => button.IsEnabled()),
            // 6 elements
            MenuV2Script.MainMenuKind.desktop_WithDiscordWishlistButtons
                or MenuV2Script.MainMenuKind.nonDesktop_WithDiscordWishlistButtons => 5 +
                                                                         _extraMainMenuButtons.Count(button => button.IsEnabled()),
            // 7 elements
            // can this even be hit?
            MenuV2Script.MainMenuKind.desktop_WIthDiscordWishlist_AndCheats
                or MenuV2Script.MainMenuKind.nonDesktop_WIthDiscordWishlist_AndCheats => 6 +
                                                                            _extraMainMenuButtons.Count(button => button.IsEnabled()),
            _ => indexquit
        };
    }

    private string[] GetMainMenuStrings(On.MenuV2Script.orig_MainMenuVoicesStringsGet orig, MenuV2Script self)
    {
        if (CurrentMenu != null)
        {
            return CurrentMenu.GetMenuStrings();
        }

        var ret = orig(self);
        var list = ret.ToList();
        var additionIndex = list.Count - 1;
        foreach (var button in _extraMainMenuButtons.Where(button => button.IsEnabled()))
        {
            list.Insert(additionIndex, button.GetText());
            button.CurrentIndex = additionIndex;
            additionIndex++;
        }

        return list.ToArray();
    }

    private void SelectMainMenuItem(On.MenuV2Script.orig__SelectMainMenu orig, MenuV2Script self)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.SelectButton(self);
            return;
        }

        switch (self.MainMenuKindGet())
        {
            // 4 elements
            case MenuV2Script.MainMenuKind.desktop_Normal:
            case MenuV2Script.MainMenuKind.nonDesktop_Normal:
                if (self.voiceIndex >= 3 && self.voiceIndex < 3 + _extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in _extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick(self);
                    }

                    return;
                }

                break;
            // 5 elements
            case MenuV2Script.MainMenuKind.desktop_WithCheats:
            case MenuV2Script.MainMenuKind.nonDesktop_WithCheats:
                if (self.voiceIndex >= 4 && self.voiceIndex < 4 + _extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in _extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick(self);
                    }

                    return;
                }

                break;
            // 6 elements
            case MenuV2Script.MainMenuKind.desktop_WithDiscordWishlistButtons:
            case MenuV2Script.MainMenuKind.nonDesktop_WithDiscordWishlistButtons:
                if (self.voiceIndex >= 5 && self.voiceIndex < 5 + _extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in _extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick(self);
                    }

                    return;
                }

                break;
            // 7 elements
            // can this even be hit?
            case MenuV2Script.MainMenuKind.desktop_WIthDiscordWishlist_AndCheats:
            case MenuV2Script.MainMenuKind.nonDesktop_WIthDiscordWishlist_AndCheats:
                if (self.voiceIndex >= 6 && self.voiceIndex < 6 + _extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in _extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick(self);
                    }

                    return;
                }

                break;
        }

        orig(self);
    }

    private string[] GetPauseMenuStrings(On.MenuV2Script.orig_PauseMenuVoicesStringsGet orig, MenuV2Script self)
    {
        if (CurrentMenu != null)
        {
            return CurrentMenu.GetMenuStrings();
        }

        var ret = orig(self);
        var list = ret.ToList();
        var additionIndex = list.Count - 1;
        foreach (var button in _extraPauseMenuButtons.Where(button => button.IsEnabled()))
        {
            list.Insert(additionIndex, button.GetText());
            button.CurrentIndex = additionIndex;
            additionIndex++;
        }

        return list.ToArray();
    }

    private void SelectPauseMenuItem(On.MenuV2Script.orig__SelectPauseMenu orig, MenuV2Script self)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.SelectButton(self);
            return;
        }

        switch (self.PauseMenuKindGet())
        {
            // 4 elements
            case MenuV2Script.PauseMenuKind.labHub_Normal:
            case MenuV2Script.PauseMenuKind.labHub_WithWishlistButtons:
                if (self.voiceIndex >= 3 && self.voiceIndex < 3 + _extraPauseMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in
                             _extraPauseMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick(self);
                    }

                    return;
                }

                break;
            // 5 elements
            default:
                if (self.voiceIndex >= 4 && self.voiceIndex < 4 + _extraPauseMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in
                             _extraPauseMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick(self);
                    }

                    return;
                }

                break;
        }

        orig(self);
    }

    private void PauseMenuDefineVoiceIndexes(On.MenuV2Script.orig__PauseMenuDefineVoiceIndexes orig, MenuV2Script self,
        MenuV2Script.PauseMenuKind pkind, out int indexwishlist, out int indexresume, out int indexrestart,
        out int indexminimap, out int indexsettings, out int indexphotomode, out int indexbacktohub,
        out int indexbacktomenu)
    {
        orig(self, pkind, out indexwishlist, out indexresume, out indexrestart, out indexminimap, out indexsettings,
            out indexphotomode, out indexbacktohub, out indexbacktomenu);

        indexbacktomenu = self.PauseMenuKindGet() switch
        {
            // 4 elements
            MenuV2Script.PauseMenuKind.labHub_Normal or MenuV2Script.PauseMenuKind.labHub_WithWishlistButtons
                => 3 + _extraPauseMenuButtons.Count(button => button.IsEnabled()),
            // 5 elements
            _ => 4 + _extraPauseMenuButtons.Count(button => button.IsEnabled())
        };
    }

    public static void AddMainMenuButton(MenuButton buttonToAdd)
    {
        if (Instance == null)
        {
            Plugin.ChaufferLogger.LogError($"Attempted to add {buttonToAdd} before menu manager was instantiated");
            return;
        }
        Instance._extraMainMenuButtons.Add(buttonToAdd);
    }

    public static void AddPauseMenuButton(MenuButton buttonToAdd)
    {
        if (Instance == null)
        {
            Plugin.ChaufferLogger.LogError($"Attempted to add {buttonToAdd} before menu manager was instantiated");
            return;
        }
        Instance._extraPauseMenuButtons.Add(buttonToAdd);
    }

    private void UpdateTexts(On.MenuV2Element.orig_UpdateTexts orig)
    {
        Plugin.ChaufferLogger.LogDebug("update texts called");
        orig();
        if (CurrentMenu == null) return;
        var textAnimatorField =
            typeof(MenuV2Element).GetField("textAnimator", BindingFlags.NonPublic | BindingFlags.Instance);
        var menuScrField = typeof(MenuV2Element).GetField("menuScr", BindingFlags.NonPublic | BindingFlags.Instance);
        if (menuScrField == null || textAnimatorField == null) return;
        foreach (var element in MenuV2Element.list)
        {
            if (!element.isSubTitle) continue;
            var menuScript = menuScrField.GetValue(element) as MenuV2Script;
            if (menuScript == null) break;
            var textAnimator = textAnimatorField.GetValue(element) as TextAnimator;
            textAnimator?.SetText(CurrentMenu.ToString(), false);
            break;
        }
    }

    private void MenuBack(On.MenuV2Script.orig_MenuBack orig, MenuV2Script self)
    {
        Plugin.ChaufferLogger.LogDebug("MenuBack called");
        if (CurrentMenu != null)
        {
            var voiceIndex = CurrentMenu.OrigVoiceIndex;
            CurrentMenu = CurrentMenu.RemoveMenu(self);
            self.menuIndex = self.menuIndex switch
            {
                MenuV2Script.indexPauseMenu => MenuV2Script.indexPauseSettings,
                MenuV2Script.indexMainMenu => MenuV2Script.indexSettings,
                _ => self.menuIndex
            };
            orig(self);
            self.voiceIndex = voiceIndex;
        }
        else orig(self);
    }
}