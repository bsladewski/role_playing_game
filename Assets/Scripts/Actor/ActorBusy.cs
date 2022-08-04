using UnityEngine;

/// <summary>
/// Keeps track of when the actor is busy.
/// </summary>
public class ActorBusy : MonoBehaviour
{
    private bool isBusy;

    /// <summary>
    /// Gets whether the actor is currently busy.
    /// </summary>
    /// /// <returns>Whether the actor is busy.</returns>
    public bool GetIsBusy()
    {
        return isBusy;
    }
}