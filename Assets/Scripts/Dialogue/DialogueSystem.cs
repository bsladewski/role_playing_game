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
    public event EventHandler<List<Actor>> OnDialogueStarted;

    /// <summary>
    /// Fired whenever dialogue between two or more actors ends.
    /// </summary>
    public event EventHandler<List<Actor>> OnDialogueEnded;

    /// <summary>
    /// Fired whenever a dialogue exchange occurs.
    /// </summary>
    public event EventHandler<DialogueExchange> OnDialogueExchange;

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
            // TODO: implement one-shot dialogues
            return;
        }

        isDialogueActive = true;
        this.actors = actors;
        OnDialogueStarted?.Invoke(gameObject, this.actors);
        OnDialogueExchange?.Invoke(gameObject, dialogueParser.Next());
    }

    /// <summary>
    /// Ends any ongoing dialogue.
    /// </summary>
    public void EndDialogue()
    {
        if (isDialogueActive)
        {
            OnDialogueEnded?.Invoke(gameObject, actors);
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

        OnDialogueExchange?.Invoke(gameObject, dialogueParser.Next());
    }
}
