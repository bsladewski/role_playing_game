using UnityEngine;

/// <summary>
/// Keeps the overhead camera positioned over the player.
/// </summary>
public class OverheadCameraController : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        Player.OnAnyPlayerDestroyed += Player_OnAnyPlayerDestroyed;
    }

    private void Player_OnAnyPlayerSpawned(Player player)
    {
        this.player = player.gameObject;
    }

    private void Player_OnAnyPlayerDestroyed(Player player)
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
