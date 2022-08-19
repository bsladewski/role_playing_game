using UnityEngine;

/// <summary>
/// Stores basic properties about an in-game item.
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Role-Playing Game/Item", order = 0)]
public class Item : ScriptableObject
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int maxStackSize = 1;

    [SerializeField]
    private Sprite itemUISprite;

    /// <summary>
    /// Gets the name of this item.
    /// </summary>
    /// <returns>The name of this item.</returns>
    public string GetItemName()
    {
        return itemName;
    }

    /// <summary>
    /// Gets the maximum size of a stack of this item.
    /// </summary>
    /// <returns>The maximum stack size.</returns>
    public int GetMaxStackSize()
    {
        return maxStackSize;
    }

    /// <summary>
    /// Gets the sprite used to display the item in the UI.
    /// </summary>
    /// <returns>The sprite used to display the item in the UI.</returns>
    public Sprite GetItemUISprite()
    {
        return itemUISprite;
    }
}
