using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A non-player character.
/// </summary>
public class NPC : Actor
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LookAtPosition lookAtPosition;

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;
        DialogueSystem.Instance.OnDialogueExchange += DialogueSystem_OnDialogueExchange;
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

    private void DialogueSystem_OnDialogueEnded(object sender, List<Actor> actors)
    {
        if (actors.Contains(this))
        {
            animator.SetBool("Talking", false);
        }
    }

    private void DialogueSystem_OnDialogueExchange(object sender, DialogueExchange exchange)
    {
        if (exchange.actorID == GetActorID())
        {
            animator.SetBool("Talking", true);
            animator.SetInteger("Emotion", ((int)exchange.emotion));
        }
        else
        {
            animator.SetBool("Talking", false);
            animator.SetInteger("Emotion", 0);
        }
    }
}
