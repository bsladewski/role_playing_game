using System.Collections;
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

    private Vector3 localScale;

    private void Awake()
    {
        deselectedAlpha = background.color.a;
    }

    private void Start()
    {
        localScale = transform.localScale;
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
            if (itemStack.GetMaxStackSize() > 999)
            {
                itemCount.text = $"{itemStack.GetStackSize()}";
            }
            else
            {
                itemCount.text = $"{itemStack.GetStackSize()}/{itemStack.GetMaxStackSize()}";
            }
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

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(PingPongScale(0.1f, 0.1f));
        }
    }

    /// <summary>
    /// Updates the background to indicate that the item is not selected.
    /// </summary>
    public void Deselect()
    {
        Color backgroundColor = background.color;
        backgroundColor.a = deselectedAlpha;
        background.color = backgroundColor;
        transform.localScale = localScale;
    }

    private IEnumerator PingPongScale(float scaleDelta, float duration)
    {
        Vector3 initialScale = localScale;
        Vector3 targetScale = initialScale + new Vector3(1f, 1f, 0f) * scaleDelta;

        for (float time = 0; time < duration * 2; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, duration) / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            yield return null;
        }
        transform.localScale = initialScale;
    }
}
