using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of whether the actor is busy. If an actor is busy they cannot perform other actions
/// such as movement.
/// </summary>
public class ActorBusy : MonoBehaviour
{
    [SerializeField]
    private Actor actor;

    private bool isBusy;

    /// <summary>
    /// Gets whether the actor is currently busy.
    /// </summary>
    /// <returns>Whether the actor is busy.</returns>
    public bool GetIsBusy()
    {
        return isBusy;
    }

    /// <summary>
    /// Sets whether the actor is currently busy.
    /// </summary>
    /// <param name="isBusy">Whether the actor is busy.</param>
    public void SetIsBusy(bool isBusy)
    {
        this.isBusy = isBusy;
    }

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;

        InventoryUI.Instance.OnInventoryOpened += InventoryUI_OnInventoryOpened;
        InventoryUI.Instance.OnInventoryClosed += InventoryUI_OnInventoryClosed;
    }

    private void DialogueSystem_OnDialogueStarted(object sender, List<Actor> actors)
    {
        if (actors.Contains(actor))
        {
            isBusy = true;
        }
    }

    private void DialogueSystem_OnDialogueEnded(object sender, List<Actor> actors)
    {
        if (actors.Contains(actor))
        {
            isBusy = false;
        }
    }

    private void InventoryUI_OnInventoryOpened(object sender, EventArgs e)
    {
        if (actor is Player)
        {
            isBusy = true;
        }
    }

    private void InventoryUI_OnInventoryClosed(object sender, EventArgs e)
    {
        if (actor is Player)
        {
            isBusy = false;
        }
    }
}
