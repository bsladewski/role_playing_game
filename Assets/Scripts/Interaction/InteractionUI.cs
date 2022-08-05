using UnityEngine;
using TMPro;

/// <summary>
/// Displays controls related to interactions.
/// </summary>
public class InteractionUI : MonoBehaviour
{
    /// <summary>
    /// An instance of this singleton class.
    /// </summary>
    /// <value></value>
    public static InteractionUI Instance { get; private set; }

    [SerializeField]
    private GameObject interactionPanel;

    [SerializeField]
    private TextMeshProUGUI interactText;

    [SerializeField]
    private TextMeshProUGUI changeInteractionText;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton Interaction UI alread exists!");
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Shows the interaction UI.
    /// </summary>
    /// <param name="interactionLabel">The label for the interact option.</param>
    /// <param name="multipleInteractions">Whether there are multiple nearby interactions.</param>
    public void Show(string interactionLabel, bool multipleInteractions)
    {
        interactText.text = $"[E] {interactionLabel}";
        if (multipleInteractions)
        {
            interactText.alignment = TextAlignmentOptions.Right;
            changeInteractionText.gameObject.SetActive(true);
        }
        else
        {
            interactText.alignment = TextAlignmentOptions.Center;
            changeInteractionText.gameObject.SetActive(false);
        }
        interactionPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the interaction UI.
    /// </summary>
    public void Hide()
    {
        interactionPanel.SetActive(false);
        changeInteractionText.gameObject.SetActive(false);
    }
}
