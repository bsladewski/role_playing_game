using UnityEngine;

/// <summary>
/// Provides helper functions for creating item stacks.
/// </summary>
public class ItemStackFactory : MonoBehaviour
{
    /// <summary>
    /// An instance of this singleton class.
    /// </summary>
    public static ItemStackFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton ItemStackFactory already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField]
    private Items items;

    /// <summary>
    /// Creates an item stack from the supplied item id and amount.
    /// </summary>
    /// <param name="itemID">The id of the item to include in the stack.</param>
    /// <param name="amount">The number of items to include in the stack.</param>
    /// <returns>A stack containing the specified item in the supplied amount.</returns>
    public ItemStack CreateItemStack(int itemID, int amount)
    {
        return new ItemStack(items.GetItem(itemID), amount);
    }
}
