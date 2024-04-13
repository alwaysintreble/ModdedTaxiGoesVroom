using System.Collections.Generic;
using System.Linq;
using RandoTaxiGoesVroom.Utils;

namespace RandoTaxiGoesVroom.Managers;

public class MenuButtonManager
{
    private readonly List<MenuButton> extraMainMenuButtons = [];
    private readonly List<MenuButton> extraPauseMenuButtons = [];
    public static MenuButtonManager Instance;
    public static CustomMenu CurrentMenu = null;

    public MenuButtonManager()
    {
        On.MenuV2Script._PauseMenuDefineVoiceIndexes += PauseMenuDefineVoiceIndexes;
        On.MenuV2Script.PauseMenuVoicesStringsGet += GetPauseMenuStrings;
        On.MenuV2Script._SelectPauseMenu += SelectPauseMenuItem;
        On.MenuV2Script._MainMenuDefineVoiceIndexes += MainMenuDefineVoiceIndexes;
        On.MenuV2Script.MainMenuVoicesStringsGet += GetMainMenuStrings;
        On.MenuV2Script._SelectMainMenu += SelectMainMenuItem;
        Instance = this;
    }

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
                extraMainMenuButtons.Count(button => button.IsEnabled()),
            // 5 elements
            MenuV2Script.MainMenuKind.desktop_WithCheats or MenuV2Script.MainMenuKind.nonDesktop_WithCheats => 4 +
                extraMainMenuButtons.Count(button => button.IsEnabled()),
            // 6 elements
            MenuV2Script.MainMenuKind.desktop_WithDiscordWishlistButtons
                or MenuV2Script.MainMenuKind.nonDesktop_WithDiscordWishlistButtons => 5 +
                extraMainMenuButtons.Count(button => button.IsEnabled()),
            // 7 elements
            // can this even be hit?
            MenuV2Script.MainMenuKind.desktop_WIthDiscordWishlist_AndCheats
                or MenuV2Script.MainMenuKind.nonDesktop_WIthDiscordWishlist_AndCheats => 6 +
                extraMainMenuButtons.Count(button => button.IsEnabled()),
            _ => indexquit
        };
    }

    private string[] GetMainMenuStrings(On.MenuV2Script.orig_MainMenuVoicesStringsGet orig, MenuV2Script self)
    {
        if (CurrentMenu is { Type: CustomMenu.MenuType.MainMenu })
        {
            return CurrentMenu.GetMenuStrings(self);
        }
        var ret = orig(self);
        var list = ret.ToList();
        var additionIndex = list.Count - 1;
        foreach (var button in extraMainMenuButtons.Where(button => button.IsEnabled()))
        {
            list.Insert(additionIndex, button.GetText());
            button.CurrentIndex = additionIndex;
            additionIndex++;
        }

        return list.ToArray();
    }

    private void SelectMainMenuItem(On.MenuV2Script.orig__SelectMainMenu orig, MenuV2Script self)
    {
        if (CurrentMenu is { Type: CustomMenu.MenuType.MainMenu })
        {
            CurrentMenu.SelectButton(self);
            return;
        }
        switch (self.MainMenuKindGet())
        {
            // 4 elements
            case MenuV2Script.MainMenuKind.desktop_Normal:
            case MenuV2Script.MainMenuKind.nonDesktop_Normal:
                if (self.voiceIndex >= 3 && self.voiceIndex < 3 + extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick();
                    }

                    return;
                }

                break;
            // 5 elements
            case MenuV2Script.MainMenuKind.desktop_WithCheats:
            case MenuV2Script.MainMenuKind.nonDesktop_WithCheats:
                if (self.voiceIndex >= 4 && self.voiceIndex < 4 + extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick();
                    }

                    return;
                }

                break;
            // 6 elements
            case MenuV2Script.MainMenuKind.desktop_WithDiscordWishlistButtons:
            case MenuV2Script.MainMenuKind.nonDesktop_WithDiscordWishlistButtons:
                if (self.voiceIndex >= 5 && self.voiceIndex < 5 + extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick();
                    }

                    return;
                }

                break;
            // 7 elements
            // can this even be hit?
            case MenuV2Script.MainMenuKind.desktop_WIthDiscordWishlist_AndCheats:
            case MenuV2Script.MainMenuKind.nonDesktop_WIthDiscordWishlist_AndCheats:
                if (self.voiceIndex >= 6 && self.voiceIndex < 6 + extraMainMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in extraMainMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick();
                    }

                    return;
                }

                break;
        }

        orig(self);
    }

    private string[] GetPauseMenuStrings(On.MenuV2Script.orig_PauseMenuVoicesStringsGet orig, MenuV2Script self)
    {
        if (CurrentMenu is { Type: CustomMenu.MenuType.PauseMenu })
        {
            return CurrentMenu.GetMenuStrings(self);
        }
        var ret = orig(self);
        var list = ret.ToList();
        var additionIndex = list.Count - 1;
        foreach (var button in extraPauseMenuButtons.Where(button => button.IsEnabled()))
        {
            list.Insert(additionIndex, button.GetText());
            button.CurrentIndex = additionIndex;
            additionIndex++;
        }

        return list.ToArray();
    }

    private void SelectPauseMenuItem(On.MenuV2Script.orig__SelectPauseMenu orig, MenuV2Script self)
    {
        if (CurrentMenu is { Type: CustomMenu.MenuType.PauseMenu })
        {
            CurrentMenu.SelectButton(self);
            return;
        }
        switch (self.PauseMenuKindGet())
        {
            // 4 elements
            case MenuV2Script.PauseMenuKind.labHub_Normal:
            case MenuV2Script.PauseMenuKind.labHub_WithWishlistButtons:
                if (self.voiceIndex >= 3 && self.voiceIndex < 3 + extraPauseMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in extraPauseMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick();
                    }

                    return;
                }

                break;
            // 5 elements
            default:
                if (self.voiceIndex >= 4 && self.voiceIndex < 4 + extraPauseMenuButtons.Count)
                {
                    Sound.Play_Unpausable("SoundMenuSelect");
                    foreach (var button in extraPauseMenuButtons.Where(button => self.voiceIndex == button.CurrentIndex))
                    {
                        button.OnClick();
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
                => 3 + extraPauseMenuButtons.Count(button => button.IsEnabled()),
            // 5 elements
            _ => 4 + extraPauseMenuButtons.Count(button => button.IsEnabled())
        };
    }

    public void AddMainMenuButton(MenuButton buttonToAdd)
    {
        extraMainMenuButtons.Add(buttonToAdd);
    }

    public void AddPauseMenuButton(MenuButton buttonToAdd)
    {
        extraPauseMenuButtons.Add(buttonToAdd);
    }
}