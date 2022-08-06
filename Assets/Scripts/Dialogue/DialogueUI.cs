using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Renders a dialogue.
/// </summary>
public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panels;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private Transform choicesPanelTransform;

    [SerializeField]
    private GameObject choicePrefab;

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;
        DialogueSystem.Instance.OnDialogueExchange += DialogueSystem_OnDialogueExchange;
    }

    private void DialogueSystem_OnDialogueStarted(object sender, List<Actor> actors)
    {
        dialogueText.text = "";
        panels.SetActive(true);
    }

    private void DialogueSystem_OnDialogueEnded(object sender, List<Actor> actors)
    {
        panels.SetActive(false);
        dialogueText.text = "";
    }

    private void DialogueSystem_OnDialogueExchange(object sender, DialogueExchange exchange)
    {
        Actor actor = DialogueSystem.Instance.GetActor(exchange.actorID);
        string text = "";
        if (actor != null)
        {
            string color = ColorUtility.ToHtmlStringRGB(actor.GetActorDisplayNameColor());
            string name = actor.GetActorDisplayName();
            text += $"<color=#{color}>{name}:</color> ";
        }
        text += exchange.text;
        dialogueText.text = text;
    }
}
