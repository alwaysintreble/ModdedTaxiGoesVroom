using System;
using System.Collections.Generic;

namespace ModdedTaxiGoesVroom.Utils;

public class MenuHelpers
{
    /// <summary>
    /// Creates a popup window for the user to enter text
    /// </summary>
    /// <param name="title">Display title of the prompt window</param>
    /// <param name="onConfirm"></param>
    /// <param name="prompt">Prompt details to provide for the user</param>
    /// <param name="input">Default text fill</param>
    /// <param name="canCancel">Whether the user can close the prompt without answering</param>
    public static void TextInput(string title, Action<string> onConfirm, string prompt = "", string input = "", bool canCancel = true)
    {
        var popup = MenuV2PopupScript.SpawnNew(title, "", prompt, canCancel);
        popup.inputField.text = input;
        popup.useInputField = true;
        if (canCancel)
            popup.BackButtonCanCloseSet();
        popup.onSimplePrompt += () => onConfirm(popup.inputField.text);
    }

    /// <summary>
    /// Creates a popup window asking a yes or no question
    /// </summary>
    /// <param name="title">Display title of the prompt window</param>
    /// <param name="onConfirm">Callback with the result of the prompt</param>
    /// <param name="prompt">The question to ask the player</param>
    /// <param name="canCancel">Whether the user can close the prompt without answering</param>
    public static void AskYesNo(string title, Action<bool> onConfirm, string prompt, bool canCancel = true)
    {
        var popup = MenuV2PopupScript.SpawnNew(title, prompt, "", canCancel, true);
        if (canCancel)
            popup.BackButtonCanCloseSet();
        popup.onAnswerYes += () =>
        {
            popup.Close(true);
            onConfirm(true);
        };
        popup.onAnswerNo += () =>
        {
            popup.Close(true);
            onConfirm(false);
        };
    }
}