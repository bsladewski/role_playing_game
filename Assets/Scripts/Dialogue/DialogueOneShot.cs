using UnityEngine;
using TMPro;

/// <summary>
/// Dislplays a single line of dialogue above an actor.
/// </summary>
public class DialogueOneShot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private Actor actor;

    private float heightOffset = 0.5f;

    /// <summary>
    /// Initializes the dialogue one-shot.
    /// </summary>
    /// <param name="actor">The actor who is speaking.</param>
    /// <param name="text">The text that will appear above the speaking actor.</param>
    /// <param name="duration">How long the one-shot will appear.</param>
    public void Initialize(Actor actor, string text, float duration)
    {
        this.actor = actor;
        dialogueText.text = text;
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        // keep the dialogue text positioned above the actor
        transform.position = Camera.main.WorldToScreenPoint(
            actor.transform.position + Vector3.up * (actor.GetActorHeight() + heightOffset)
        );
    }
}
