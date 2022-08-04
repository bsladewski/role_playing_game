using System;
using UnityEngine;

/// <summary>
/// Represents the player character.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// Fired whenever a player object spawns.
    /// </summary>
    public static event EventHandler<Player> OnAnyPlayerSpawned;

    /// <summary>
    /// Fired whenever a player is destroyed.
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

    private void Update()
    {
        Debug.Log(transform.forward);
        Debug.DrawLine(transform.position, transform.forward * 2f, Color.red, 1f);
    }
}
