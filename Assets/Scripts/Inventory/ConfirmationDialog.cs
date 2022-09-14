using System;
using UnityEngine;

/// <summary>
/// Confirms a player action.
/// </summary>
public class ConfirmationDialog : MonoBehaviour
{
    /// <summary>
    /// Fired when the chooses to confirm or decline an action.
    /// </summary>
    public Action OnConfirmationComplete;

    private Action callback;

    private bool isConfirmed;

    /// <summary>
    /// Sets the confirmation dialog text and callback function.
    /// </summary>
    /// <param name="confirmationText">The text displayed on the confirmation dialog.</param>
    /// <param name="callback">The action to perform if the player confirms the action.</param>
    public void Initialize(string confirmationText, Action callback)
    {
        isConfirmed = true;
        this.callback = callback;
    }

    private void Confirm()
    {
        if (isConfirmed)
        {
            callback();
        }
        OnConfirmationComplete?.Invoke();
    }
}
