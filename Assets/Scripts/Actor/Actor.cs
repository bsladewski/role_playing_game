using UnityEngine;

/// <summary>
/// The base class for objects that take part in dialogue.
/// </summary>
public class Actor : MonoBehaviour
{
    [HeaderAttribute("Actor Properties")]

    [SerializeField]
    private string actorID;

    [SerializeField]
    private string actorDisplayName;

    [SerializeField]
    private Color actorDisplayNameColor = Color.white;

    [SerializeField]
    private float actorHeight = 1.8f;

    /// <summary>
    /// Gets the id used to identify this actor during a dialogue.
    /// </summary>
    /// <returns>The actor id.</returns>
    public string GetActorID()
    {
        return actorID;
    }

    /// <summary>
    /// The display name used for this actor during dialogue.
    /// </summary>
    /// <returns>The actor display name.</returns>
    public string GetActorDisplayName()
    {
        return actorDisplayName;
    }

    /// <summary>
    /// A color that helps distinguish this actor during dialogue.
    /// </summary>
    /// /// <returns>The color of this actor's display name.</returns>
    public Color GetActorDisplayNameColor()
    {
        return actorDisplayNameColor;
    }

    /// <summary>
    /// The height of the actor.
    /// </summary>
    /// <returns>The height of the actor.</returns>
    public float GetActorHeight()
    {
        return actorHeight;
    }
}
