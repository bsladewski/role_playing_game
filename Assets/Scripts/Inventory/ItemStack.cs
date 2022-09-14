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
    /// Returns the name of the item stored within this stack.
    /// </summary>
    /// <returns>The name of the item stored within this stack.</returns>
    public string GetItemName()
    {
        return item.GetItemName();
    }

    /// <summary>
    /// Returns a description of the item stored within this stack.
    /// </summary>
    /// <returns>A description of the item stored within this stack.</returns>
    public string GetItemDescription()
    {
        return item.GetItemDescription();
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
    /// Returns the number of items that can be added to this stack.
    /// </summary>
    /// <returns>The number of items that can be added to this stack.</returns>
    public int GetRemainingCapacity()
    {
        int remainingCapacity = GetMaxStackSize() - GetStackSize();
        if (remainingCapacity < 0)
        {
            remainingCapacity = 0;
        }
        return remainingCapacity;
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
    /// Gets the color of the sprite used to display this item in the UI.
    /// </summary>
    /// <returns>The color of the sprite used to display this item in the UI.</returns>
    public Color GetItemUISpriteColor()
    {
        return item.GetItemUISpriteColor();
    }

    /// <summary>
    /// Transfers a number of items from this stack to another stack.
    /// </summary>
    /// <param name="other">The stack to transfer to.</param>
    /// <param name="amount">The number of items to transfer.</param>
    public void Transfer(ItemStack other, int amount)
    {
        int remainingCapacity = other.GetRemainingCapacity();
        int amountToTransfer = Mathf.Min(stackSize, remainingCapacity, amount);
        if (amountToTransfer <= 0)
        {
            return;
        }

        stackSize -= amountToTransfer;
        other.stackSize += amountToTransfer;
    }

    /// <summary>
    /// Sets the number of items in this stack.
    /// </summary>
    public void SetStackSize(int amount)
    {
        stackSize = Mathf.Min(GetMaxStackSize(), amount);
    }
}
