using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interactions that should initiate dialogue.
/// </summary>
public class NPCDialogueInteractionHandler : MonoBehaviour
{
    [SerializeField]
    private NPC npc;

    [SerializeField]
    private Interaction interaction;

    [SerializeField]
    private TextAsset dialogueText;

    private void Start()
    {
        interaction.OnInteractionPerformed += Interaction_OnInteractionPerformed;
    }

    private void Interaction_OnInteractionPerformed(Player player)
    {
        DialogueSystem.Instance.StartDialogue(
            new List<Actor>() { npc, player },
            dialogueText
        );
    }
}
