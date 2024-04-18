using System;

namespace ModdedTaxiGoesVroom.Utils;

public class MenuButton(Func<string> getText, Action<MenuV2Script> onClick, Func<bool> isEnabled = null)
{
    public readonly Action<MenuV2Script> OnClick = onClick;
    public readonly Func<string> GetText = getText;
    public readonly Func<bool> IsEnabled = isEnabled;
    public int CurrentIndex = -1;

    public MenuButton(string text, Action<MenuV2Script> onClick, Func<bool> isEnabled = null) : this(() => text,
        onClick,
        isEnabled)
    {
    }

    public MenuButton(Func<string> getText, Action<MenuV2Script> onClick) : this(getText, onClick, () => true)
    {
    }

    public MenuButton(string text, Action<MenuV2Script> onClick) : this(() => text, onClick, () => true)
    {
    }

    public MenuButton(string text) : this(() => text, null, () => true)
    {
    }
}