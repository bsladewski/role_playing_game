using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a collection of items owned by this game object.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private string title = "Container";

    [SerializeField]
    private int maxSlots = 25;

    private List<ItemStack> itemStacks;

    private void Awake()
    {
        itemStacks = new List<ItemStack>();
    }

    /// <summary>
    /// Gets a display name for this inventory.
    /// </summary>
    /// <returns>A display name for this inventory.</returns>
    public string GetTitle()
    {
        return title;
    }

    /// <summary>
    /// Gets an array containing all items in the inventory.
    /// </summary>
    /// <returns>An array containing all items in the inventory.</returns>
    public ItemStack[] GetItems()
    {
        return itemStacks.ToArray();
    }

    /// <summary>
    /// Adds an item stack to the inventory. This function will attempt to add as many items as
    /// possible to inventory. The amount of items in the item stack will be modified. If the amount
    /// of items remaining in the stack is zero all items where added. If the amount of items
    /// remaining in the stack is greater than zero we did not add the entire stack to the inventory
    /// because the inventory became full.
    /// </summary>
    /// <param name="itemStack">The stack of items to add to the inventory.</param>
    /// <param name="amount">The number of items to add to the inventory.</param>
    public void AddItem(ItemStack itemStack, int amount)
    {
        foreach (ItemStack other in itemStacks)
        {
            if (itemStack.GetStackSize() <= 0)
            {
                break;
            }

            if (itemStack.GetItem() == other.GetItem())
            {
                itemStack.Transfer(other, amount);
            }
        }

        if (itemStack.GetStackSize() > 0 && itemStacks.Count < maxSlots)
        {
            ItemStack newItemStack = new ItemStack(itemStack.GetItem(), 0);
            itemStack.Transfer(newItemStack, amount);
            itemStacks.Add(newItemStack);
        }

        Prune();
    }

    /// <summary>
    /// Adds an item stack to the inventory.
    /// </summary>
    /// <param name="itemStack">The stack of items to add to the inventory.</param>
    public void AddItem(ItemStack itemStack)
    {
        AddItem(itemStack, itemStack.GetStackSize());
    }

    /// <summary>
    /// Removes an item stack from this inventory.
    /// </summary>
    /// <param name="itemStack">The item stack to remove.</param>
    public void RemoveItemStack(ItemStack itemStack)
    {
        itemStack.SetStackSize(0);
        Prune();
    }

    /// <summary>
    /// Transfers a number of items from this inventory to another inventory.
    /// </summary>
    /// <param name="itemStack">The item stack to transfer.</param>
    /// <param name="to">The inventory to transfer items to.</param>
    /// <param name="amount">The number of items to transfer.</param>
    public void TransferItem(ItemStack itemStack, Inventory to, int amount)
    {
        if (!itemStacks.Contains(itemStack))
        {
            return;
        }

        to.AddItem(itemStack, amount);
        Prune();
    }

    /// <summary>
    /// Gets the number of item stacks in the inventory.
    /// </summary>
    /// <returns>The number of item stacks in the inventory.</returns>
    public int GetItemStackCount()
    {
        return itemStacks.Count;
    }

    /// <summary>
    /// Get the maximum number of inventory slots.
    /// </summary>
    /// <returns>The maximum number of inventory slots.</returns>
    public int GetMaxCapacity()
    {
        return maxSlots;
    }

    /// <summary>
    /// Gets the number of unfilled inventory slots.
    /// </summary>
    /// <returns>The number of unfilled inventory slots.</returns>
    public int GetRemainingCapacity()
    {
        return maxSlots - itemStacks.Count;
    }

    private void Prune()
    {
        itemStacks.RemoveAll(itemStack => itemStack.GetStackSize() <= 0);
    }
}
