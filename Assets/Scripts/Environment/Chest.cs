using UnityEngine;

/// <summary>
/// Represents a lootable chest.
/// </summary>
public class Chest : MonoBehaviour
{
    [SerializeField]
    private Interaction interaction;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Transform lid;

    private bool isOpen;

    private void Start()
    {
        interaction.OnInteractionPerformed += Interaction_OnInteractionPerformed;
    }

    private void Interaction_OnInteractionPerformed(object sender, Player player)
    {
        ActorBusy playerActorBusy = player.GetComponent<ActorBusy>();
        if (!playerActorBusy.GetIsBusy())
        {
            if (!isOpen && lid != null)
            {
                lid.eulerAngles = new Vector3(-90f, 0f, 0f);
                isOpen = true;
            }

            InventoryUI.Instance.OpenInventory(player.GetPlayerInventory(), inventory);
        }
    }
}
