using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays a stack of items in the inventory UI.
/// </summary>
public class ItemStackUI : MonoBehaviour
{
    [SerializeField]
    private Image background;

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI itemCount;

    [SerializeField]
    private float selectedAlpha = 0.4f;

    private float deselectedAlpha;

    private ItemStack itemStack;

    private void Awake()
    {
        deselectedAlpha = background.color.a;
    }

    /// <summary>
    /// Gets the item stack displayed by this object.
    /// </summary>
    /// <returns>The item stack displayed by this object.</returns>
    public ItemStack GetItemStack()
    {
        return itemStack;
    }

    /// <summary>
    /// Sets the item stack displayed by this object. If null is supplied this object will display
    /// an empty inventory slot.
    /// </summary>
    /// <param name="itemStack">The new item stack displayed by this object.</param>
    public void SetItemStack(ItemStack itemStack)
    {
        this.itemStack = itemStack;
        if (itemStack == null)
        {
            itemImage.gameObject.SetActive(false);
            itemCount.gameObject.SetActive(false);
            return;
        }
        else
        {
            itemImage.sprite = itemStack.GetItemUISprite();
            itemCount.text = $"{itemStack.GetStackSize()}/{itemStack.GetMaxStackSize()}";
            itemImage.gameObject.SetActive(true);
            itemCount.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Updates the background to indicate that the item is selected.
    /// </summary>
    public void Select()
    {
        Color backgroundColor = background.color;
        backgroundColor.a = selectedAlpha;
        background.color = backgroundColor;
    }

    /// <summary>
    /// Updates the background to indicate that the item is not selected.
    /// </summary>
    public void Deselect()
    {
        Color backgroundColor = background.color;
        backgroundColor.a = deselectedAlpha;
        background.color = backgroundColor;
    }
}
