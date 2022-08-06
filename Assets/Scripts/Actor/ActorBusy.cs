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

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;
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
}
