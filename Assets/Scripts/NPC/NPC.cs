using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A non-player character.
/// </summary>
public class NPC : Actor
{
    [SerializeField]
    private LookAtPosition lookAtPosition;

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
    }

    private void DialogueSystem_OnDialogueStarted(object sender, List<Actor> actors)
    {
        if (actors.Contains(this))
        {
            // find and look at the player character
            foreach (Actor actor in actors)
            {
                if (actor is Player)
                {
                    lookAtPosition.SetTargetPosition(actor.transform.position);
                    break;
                }
            }
        }
    }
}
