using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Allows the player to select and perform nearby interactions.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// Fired whenever the selected interaction changes.
    /// </summary>
    public static event EventHandler<Interaction> OnAnySelectedInteractionChanged;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private ActorBusy actorBusy;

    [SerializeField]
    private LayerMask interactionLayerMask;

    [SerializeField]
    private float interactionRadius = 3f;

    private InputAction performInteractionAction;
    private InputAction changeInteractionAction;

    private Interaction selectedInteraction;

    private bool shouldPerformAction;
    private bool shouldChangeSelection;
    private bool isMultipleInteractions;

    private void Start()
    {
        performInteractionAction = playerInput.actions["Perform Interaction"];
        changeInteractionAction = playerInput.actions["Change Interaction"];

        performInteractionAction.started += _ =>
        {
            if (!actorBusy.GetIsBusy())
            {
                shouldPerformAction = true;
            }
        };

        changeInteractionAction.started += _ =>
        {
            if (!actorBusy.GetIsBusy())
            {
                shouldChangeSelection = true;
            }
        };
    }

    private void Update()
    {
        if (actorBusy.GetIsBusy())
        {
            if (selectedInteraction != null)
            {
                selectedInteraction = null;
                OnAnySelectedInteractionChanged?.Invoke(gameObject, null);
            }
            return;
        }

        HandleGetSelectedInteraction();
    }

    private void HandleGetSelectedInteraction()
    {
        // find nearby colliders in the interaction layer
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            interactionRadius,
            interactionLayerMask
        );

        List<Interaction> interactions = new List<Interaction>();

        foreach (Collider collider in colliders)
        {
            // populate the list of interactions
            Interaction interaction = collider.gameObject.GetComponent<Interaction>();
            if (interaction != null && interaction.GetCanPerformInteraction())
            {
                interactions.Add(interaction);
            }
        }

        bool dirty = false;

        if (isMultipleInteractions != interactions.Count > 1)
        {
            isMultipleInteractions = interactions.Count > 1;
            dirty = true;
        }

        // find the next selected interaction
        Interaction nextSelectionInteraction = FindSelectedInteraction(interactions);
        if (selectedInteraction != nextSelectionInteraction)
        {
            // if the selected action has changed update the selected interaction
            selectedInteraction = nextSelectionInteraction;
            OnAnySelectedInteractionChanged?.Invoke(gameObject, selectedInteraction);
            dirty = true;
        }

        if (dirty)
        {
            // show or hide the interaction UI
            if (selectedInteraction == null)
            {
                InteractionUI.Instance.Hide();
            }
            else
            {
                InteractionUI.Instance.Show(
                    selectedInteraction.GetInteractionLabel(),
                    interactions.Count > 1
                );
            }
        }

        // perform the interaction if possible
        if (
            selectedInteraction != null &&
            selectedInteraction.GetCanPerformInteraction() &&
            shouldPerformAction)
        {
            shouldPerformAction = false;
            selectedInteraction.PerformInteraction();
        }
    }

    private Interaction FindSelectedInteraction(List<Interaction> interactions)
    {
        if (interactions.Count > 2)
        {
            // if there are more than two interactions nearby sort them by distance to the player
            interactions = interactions.OrderBy(interaction =>
            {
                return Vector3.Distance(transform.position, interaction.transform.position);
            }).ToList();
        }

        int idx = -1;
        for (int i = 0; selectedInteraction != null && i < interactions.Count; i++)
        {
            // search for the currently selected interaction
            if (interactions[i] == selectedInteraction)
            {
                idx = i;
            }
        }

        if (idx < 0)
        {
            // if the selected interaction was not found return the first element or null
            return interactions.Count > 0 ? interactions[0] : null;
        }

        if (shouldChangeSelection)
        {
            // if we should change our selection get the next item in the list
            // if we are at the end of the list get the first item in the list
            shouldChangeSelection = false;
            return idx + 1 < interactions.Count ? interactions[idx + 1] : interactions[0];
        }

        return interactions[idx];
    }
}
