using UnityEngine;

/// <summary>
/// Stores basic properties about an in-game item.
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Role-Playing Game/Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField]
    private int itemID;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private Sprite itemUISprite;

    [SerializeField]
    private Color itemUISpriteColor = Color.white;

    [SerializeField]
    private int maxStackSize = 1;

    [SerializeField]
    [TextArea]
    private string itemDescription;

    /// <summary>
    /// Gets an identifier for this item.
    /// </summary>
    /// <returns>An identifier for this item.</returns>
    public int GetItemID()
    {
        return itemID;
    }

    /// <summary>
    /// Gets the name of this item.
    /// </summary>
    /// <returns>The name of this item.</returns>
    public string GetItemName()
    {
        return itemName;
    }

    /// <summary>
    /// Gets a description of this item.
    /// </summary>
    /// <returns>A description of this item.</returns>
    public string GetItemDescription()
    {
        return itemDescription;
    }

    /// <summary>
    /// Gets the sprite used to display the item in the UI.
    /// </summary>
    /// <returns>The sprite used to display the item in the UI.</returns>
    public Sprite GetItemUISprite()
    {
        return itemUISprite;
    }

    /// <summary>
    /// Gets the color of the sprite used to display the item in the UI.
    /// </summary>
    /// <returns>The color of the sprite used to display the item in the UI.</returns>
    public Color GetItemUISpriteColor()
    {
        return itemUISpriteColor;
    }

    /// <summary>
    /// Gets the maximum size of a stack of this item.
    /// </summary>
    /// <returns>The maximum stack size.</returns>
    public int GetMaxStackSize()
    {
        return maxStackSize;
    }
}
