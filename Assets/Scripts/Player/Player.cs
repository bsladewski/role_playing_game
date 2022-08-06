using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the player character.
/// </summary>
public class Player : Actor
{
    /// <summary>
    /// Fired whenever a player object spawns.
    /// </summary>
    public static event EventHandler<Player> OnAnyPlayerSpawned;

    /// <summary>
    /// Fired whenever a player object is destroyed.
    /// </summary>
    public static event EventHandler<Player> OnAnyPlayerDestroyed;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LookAtPosition lookAtPosition;

    private void Start()
    {
        OnAnyPlayerSpawned?.Invoke(gameObject, this);
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;
        DialogueSystem.Instance.OnDialogueExchange += DialogueSystem_OnDialogueExchange;
    }

    private void OnDestroy()
    {
        OnAnyPlayerDestroyed?.Invoke(gameObject, this);
    }

    private void DialogueSystem_OnDialogueStarted(object sender, List<Actor> actors)
    {
        if (actors.Contains(this))
        {
            // find and look at the first NPC
            foreach (Actor actor in actors)
            {
                if (actor is NPC)
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

        Actor actor = DialogueSystem.Instance.GetActor(exchange.actorID);
        if (actor != null && actor is NPC)
        {
            // look at the NPC who is speaking
            lookAtPosition.SetTargetPosition(actor.transform.position);
        }
    }
}
