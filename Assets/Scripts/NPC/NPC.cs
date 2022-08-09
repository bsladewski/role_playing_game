using System.Collections;
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
    private ActorBusy actorBusy;

    [SerializeField]
    private LookAtPosition lookAtPosition;

    [SerializeField]
    private DialogueOneShot dialogueOneShotPrefab;

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;
        DialogueSystem.Instance.OnDialogueExchange += DialogueSystem_OnDialogueExchange;
        DialogueSystem.Instance.OnDialogueOneShot += DialogueSystem_OnDialogueOneShot;
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

    private void DialogueSystem_OnDialogueOneShot(object sender, DialogueExchange exchange)
    {
        if (exchange.actorID != GetActorID())
        {
            return;
        }

        float dialogueOneShotDuration = 3f;
        GameObject uiCanvas = GameObject.FindGameObjectWithTag("UI Canvas");
        DialogueOneShot dialogueOneShot = Instantiate<DialogueOneShot>(dialogueOneShotPrefab);
        dialogueOneShot.Initialize(this, exchange.text, dialogueOneShotDuration);
        dialogueOneShot.transform.SetParent(uiCanvas.transform);
        actorBusy.SetIsBusy(true);
        animator.SetBool("Talking", true);
        animator.SetInteger("Emotion", (int)exchange.emotion);
        StartCoroutine(Delay(dialogueOneShotDuration, () =>
        {
            animator.SetBool("Talking", false);
            animator.SetInteger("Emotion", 0);
            actorBusy.SetIsBusy(false);
        }));
    }

    private IEnumerator Delay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
