using UnityEngine;

/// <summary>
/// Keeps track of whether the actor is busy. If an actor is busy they cannot perform other actions
/// such as movement.
/// </summary>
public class ActorBusy : MonoBehaviour
{
    private bool isBusy;

    /// <summary>
    /// Gets whether the actor is currently busy.
    /// </summary>
    /// <returns>Whether the actor is busy.</returns>
    public bool GetIsBusy()
    {
        return isBusy;
    }
}