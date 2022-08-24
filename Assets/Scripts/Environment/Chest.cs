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

        // TODO: remove test items
        inventory.AddItem(ItemStackFactory.Instance.CreateItemStack(0, 200));
        inventory.AddItem(ItemStackFactory.Instance.CreateItemStack(1, 4));
        inventory.AddItem(ItemStackFactory.Instance.CreateItemStack(2, 2));
    }

    private void Interaction_OnInteractionPerformed(Player player)
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
