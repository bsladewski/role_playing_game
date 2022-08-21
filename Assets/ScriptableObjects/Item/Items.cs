using System;
using UnityEngine;

/// <summary>
/// Stores an array of item scriptable objects.
/// </summary>
[CreateAssetMenu(fileName = "Items", menuName = "Role-Playing Game/Items", order = 0)]
public class Items : ScriptableObject
{
    [SerializeField]
    private Item[] items;

    /// <summary>
    /// Get an item by its item id.
    /// </summary>
    /// <param name="itemID">The id of the item to fetch.</param>
    /// <returns>The item that corresponds to the supplied item id.</returns>
    public Item GetItem(int itemID)
    {
        foreach (Item item in items)
        {
            if (item.GetItemID() == itemID)
            {
                return item;
            }
        }
        throw new Exception($"no such item id: {itemID}");
    }
}
