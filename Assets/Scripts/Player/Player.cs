using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    /// <summary>
    /// Fired whenever a player enters a building.
    /// </summary>
    public static event EventHandler<Building> OnAnyBuildingEntered;

    /// <summary>
    /// Fired whenever a player exits a building.
    /// </summary>
    public static event EventHandler<Building> OnAnyBuildingExited;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LookAtPosition lookAtPosition;

    [SerializeField]
    private LayerMask roofLayerMask;

    [SerializeField]
    private ActorBusy actorBusy;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private Inventory inventory;

    private Building currentBuilding;

    private InputAction openInventoryAction;

    /// <summary>
    /// Gets the player inventory.
    /// </summary>
    /// <returns>The player inventory.</returns>
    public Inventory GetPlayerInventory()
    {
        return inventory;
    }

    private void Start()
    {
        OnAnyPlayerSpawned?.Invoke(gameObject, this);
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;
        DialogueSystem.Instance.OnDialogueExchange += DialogueSystem_OnDialogueExchange;
        inventory.AddItem(ItemStackFactory.Instance.CreateItemStack(0, 100));

        openInventoryAction = playerInput.actions["Open Inventory"];

        openInventoryAction.started += _ =>
        {
            if (!actorBusy.GetIsBusy())
            {
                InventoryUI.Instance.OpenInventory(inventory);
            }
        };
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 10f, roofLayerMask))
        {
            Building next = hit.collider.gameObject.GetComponentInParent<Building>();
            if (next != currentBuilding)
            {
                currentBuilding = next;
                OnAnyBuildingEntered?.Invoke(gameObject, currentBuilding);
            }
        }
        else if (currentBuilding != null)
        {
            OnAnyBuildingExited?.Invoke(gameObject, currentBuilding);
            currentBuilding = null;
        }
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
