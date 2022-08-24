using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Coordinates dialogue between actors.
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    /// <summary>
    /// An instance of this singleton class.
    /// </summary>
    /// <value></value>
    public static DialogueSystem Instance { get; private set; }

    /// <summary>
    /// Fired whenever dialogue between two or more actors starts.
    /// </summary>
    public event Action<List<Actor>> OnDialogueStarted;

    /// <summary>
    /// Fired whenever dialogue between two or more actors ends.
    /// </summary>
    public event Action<List<Actor>> OnDialogueEnded;

    /// <summary>
    /// Fired whenever a dialogue exchange occurs.
    /// </summary>
    public event Action<DialogueExchange> OnDialogueExchange;

    /// <summary>
    /// Fired whenever a dialogue one-shot occurs.
    /// </summary>
    public event Action<DialogueExchange> OnDialogueOneShot;

    [SerializeField]
    private PlayerInput playerInput;

    private InputAction selectAction;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton DialogueSystem already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        selectAction = playerInput.actions["Select"];

        selectAction.started += _ => HandleSelectAction();
    }

    private bool isDialogueActive;

    private DialogueParser dialogueParser;

    private List<Actor> actors;

    /// <summary>
    /// Initiates dialogue between the specified actors.
    /// </summary>
    public void StartDialogue(List<Actor> actors, TextAsset dialogueText)
    {
        dialogueParser = new DialogueParser(dialogueText);
        if (dialogueParser.IsOneShot())
        {
            OnDialogueOneShot?.Invoke(dialogueParser.Next());
            return;
        }

        isDialogueActive = true;
        this.actors = actors;
        OnDialogueStarted?.Invoke(this.actors);
        OnDialogueExchange?.Invoke(dialogueParser.Next());
    }

    /// <summary>
    /// Ends any ongoing dialogue.
    /// </summary>
    public void EndDialogue()
    {
        if (isDialogueActive)
        {
            OnDialogueEnded?.Invoke(actors);
            isDialogueActive = false;
            actors = null;
            dialogueParser = null;
        }
    }

    /// <summary>
    /// Gets an actor that is participating in the current dialogue.
    /// </summary>
    /// <param name="actorID">The actor id.</param>
    /// <returns>An actor or null.</returns>
    public Actor GetActor(string actorID)
    {
        foreach (Actor actor in actors)
        {
            if (actor.GetActorID() == actorID)
            {
                return actor;
            }
        }
        return null;
    }

    private void HandleSelectAction()
    {
        if (!isDialogueActive)
        {
            return;
        }

        if (!dialogueParser.HasNext())
        {
            EndDialogue();
            return;
        }

        OnDialogueExchange?.Invoke(dialogueParser.Next());
    }
}
