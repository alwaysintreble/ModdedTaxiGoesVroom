using System;
using UnityEngine.Events;

namespace RandoTaxiGoesVroom.Utils;

public class MenuButton(Func<string> getText, UnityAction onClick, Func<bool> isEnabled = null)
{
    public readonly UnityAction OnClick = onClick;
    public readonly Func<string> GetText = getText;
    public readonly Func<bool> IsEnabled = isEnabled;
    public int CurrentIndex = -1;
}