using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Allows the player character to interact with this object.
/// </summary>
public class Interaction : MonoBehaviour
{
    /// <summary>
    /// Fired whenever this interaction is performed.
    /// </summary>
    public event EventHandler<Player> OnInteractionPerformed;

    [SerializeField]
    private string interactionLabel = "Interact";

    [SerializeField]
    private Outline outline;

    [SerializeField]
    private bool isOneShot;

    [SerializeField]
    private float cooldown;

    private int interactionCount;

    private bool cooldownActive;

    private void Start()
    {
        PlayerInteraction.OnAnySelectedInteractionChanged += PlayerInteraction_OnAnySelectedInteractionChanged;
    }

    /// <summary>
    /// Gets a label that describes this interaction.
    /// </summary>
    /// <returns>Gets a label for this interaction.</returns>
    public string GetInteractionLabel()
    {
        return interactionLabel;
    }

    /// <summary>
    /// Gets whether this interaction can be performed.
    /// </summary>
    /// <returns>Whether this interaction can be performed.</returns>
    public bool GetCanPerformInteraction()
    {
        if ((isOneShot && interactionCount > 0) || cooldownActive)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Triggers the event to perform this interaction.
    /// </summary>
    public void PerformInteraction(Player player)
    {
        OnInteractionPerformed?.Invoke(gameObject, player);
        interactionCount++;
        cooldownActive = true;
        StartCoroutine(Delay(cooldown, () => cooldownActive = false));
    }

    private IEnumerator Delay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    private void PlayerInteraction_OnAnySelectedInteractionChanged(object sender, Interaction interaction)
    {
        if (outline != null)
        {
            outline.enabled = interaction == this;
        }
    }
}
