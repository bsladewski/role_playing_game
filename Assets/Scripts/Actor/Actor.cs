using UnityEngine;

/// <summary>
/// The base class for objects that take part in dialogue.
/// </summary>
public abstract class Actor : MonoBehaviour
{
    /// <summary>
    /// Used to identify this actor in a dialogue.
    /// </summary>
    public abstract string GetActorID();

    /// <summary>
    /// The display name used for this actor in a dialogue.
    /// </summary>
    /// <returns>The actor display name.</returns>
    public abstract string GetActorDisplayName();

    /// <summary>
    /// A color that helps distinguish this actor during dialogue.
    /// </summary>
    /// <returns>A color that helps identify this actor.</returns>
    public abstract Color GetActorColor();
}