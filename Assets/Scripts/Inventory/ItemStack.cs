using UnityEngine;

/// <summary>
/// Represents a stack of items.
/// </summary>
public class ItemStack
{
    private Item item;

    private int stackSize;

    public ItemStack(Item item, int stackSize)
    {
        if (stackSize > item.GetMaxStackSize())
        {
            Debug.LogError(
                $"Item '{item.GetItemName()}' stack size greater than '{item.GetMaxStackSize()}', truncating."
            );
            stackSize = item.GetMaxStackSize();
        }
        this.item = item;
        this.stackSize = stackSize;
    }

    /// <summary>
    /// Returns the item stored within this stack.
    /// </summary>
    /// <returns>The item stored within this stack.</returns>
    public Item GetItem()
    {
        return item;
    }

    /// <summary>
    /// Gets the size of this stack.
    /// </summary>
    /// <returns>The size of this stack.</returns>
    public int GetStackSize()
    {
        return stackSize;
    }

    /// <summary>
    /// Gets the maximum size of this stack.
    /// </summary>
    /// <returns>The maximum size of this stack.</returns>
    public int GetMaxStackSize()
    {
        return item.GetMaxStackSize();
    }

    /// <summary>
    /// Gets the sprite used to display this item in the UI.
    /// </summary>
    /// <returns>The sprite used to display this item in the UI.</returns>
    public Sprite GetItemUISprite()
    {
        return item.GetItemUISprite();
    }

    /// <summary>
    /// Transfers items from this stack to another stack.
    /// </summary>
    /// <param name="other">The stack to transfer to.</param>
    public void Transfer(ItemStack other)
    {
        int remainingCapacity = item.GetMaxStackSize() - stackSize;
        int amountToTransfer = Mathf.Min(other.stackSize, stackSize);
        if (amountToTransfer <= 0)
        {
            return;
        }

        stackSize += amountToTransfer;
        other.stackSize -= amountToTransfer;
    }
}
