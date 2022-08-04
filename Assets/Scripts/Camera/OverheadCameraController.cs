using UnityEngine;

/// <summary>
/// Keeps the overhead camera position over the player.
/// </summary>
public class OverheadCameraController : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        Player.OnAnyPlayerDestroyed += Player_OnAnyPlayerDestroyed;
    }

    private void Player_OnAnyPlayerSpawned(object sender, Player player)
    {
        this.player = player.gameObject;
    }

    private void Player_OnAnyPlayerDestroyed(object sender, Player player)
    {
        this.player = null;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        transform.position = player.transform.position;
    }
}
