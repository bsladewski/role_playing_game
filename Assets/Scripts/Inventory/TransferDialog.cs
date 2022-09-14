using System;
using UnityEngine;

/// <summary>
/// Allows the player to select a specific number of items to transfer between inventories.
/// </summary>
public class TransferDialog : MonoBehaviour
{
    /// <summary>
    /// Fired when the player chooses to transfer a number of items.
    /// </summary>
    public Action<Inventory, Inventory, ItemStack, int> OnTransferSelected;

    /// <summary>
    /// Fired when the player exits the transfer dialog without transferring any items.
    /// </summary>
    public Action OnTransferCancelled;

    private int maxItems;

    private int selectedItems;

    /// <summary>
    /// Sets the maximum number of items the player can transfer and resets the number of items to
    /// transfer.
    /// </summary>
    /// <param name="from">The inventory to transfer from.</param>
    /// <param name="to">The inventory to trasnfer to.</param>
    /// <param name="itemStack">The stack the player is trying to transfer.</param>
    public void Initialize(Inventory from, Inventory to, ItemStack itemStack)
    {
        this.maxItems = itemStack.GetStackSize();
        selectedItems = 1;
    }
}
