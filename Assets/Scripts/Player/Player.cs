using System;

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

    private void Start()
    {
        OnAnyPlayerSpawned?.Invoke(gameObject, this);
    }

    private void OnDestroy()
    {
        OnAnyPlayerDestroyed?.Invoke(gameObject, this);
    }
}
