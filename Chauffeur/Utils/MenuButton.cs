using System;

namespace Chauffeur.Utils;

public class MenuButton
{
    public readonly Action<MenuV2Script> OnClick;
    public readonly Func<string> GetText;
    public readonly Func<bool> IsEnabled;
    public int CurrentIndex = -1;

    public MenuButton(Func<string> getText, Action<MenuV2Script> onClick = null, Func<bool> isEnabled = null)
    {
        GetText = getText;
        OnClick = onClick ?? (_ => { });
        IsEnabled = isEnabled ?? (() => true);
    }

    public MenuButton(string text, Action<MenuV2Script> onClick = null, Func<bool> isEnabled = null) : this(() => text,
        onClick, isEnabled)
    {
    }

    public MenuButton(Func<string> getText, Action onClick = null, Func<bool> isEnabled = null)
    {
        GetText = getText;
        OnClick = onClick == null ? _ => { } : _ => onClick();
        IsEnabled = isEnabled ?? (() => true);
    }

    public MenuButton(string text, Action onClick = null, Func<bool> isEnabled = null) : this(() => text, onClick,
        isEnabled)
    {
    }
}