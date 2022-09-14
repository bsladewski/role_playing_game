using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Displays inventories that the player has access to.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// An instance of this singleton class.
    /// </summary>
    public static InventoryUI Instance { get; private set; }

    private static readonly int TWO_COLUMN_GRID_WIDTH = 4;

    private static readonly int THREE_COLUMN_GRID_WIDTH = 3;

    /// <summary>
    /// Fired whenever the inventory is opened.
    /// </summary>
    public event Action OnInventoryOpened;

    /// <summary>
    /// Fired whenever the inventory is closed.
    /// </summary>
    public event Action OnInventoryClosed;

    [SerializeField]
    private GameObject inventoryUIPanel;

    [SerializeField]
    private ItemStackUI itemStackUIPrefab;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private TextMeshProUGUI selectedItemTitle;

    [SerializeField]
    private TextMeshProUGUI selectedItemDescription;

    [HeaderAttribute("Player Inventory")]
    [SerializeField]
    private TextMeshProUGUI playerInventoryTitle;

    [SerializeField]
    private ScrollRect playerInventoryScrollRect;

    [SerializeField]
    private RectTransform playerInventoryViewportRectTransform;

    [SerializeField]
    private GridLayoutGroup playerInventoryGridLayoutGroup;

    [HeaderAttribute("Other Inventory")]
    [SerializeField]
    private GameObject otherInventoryPanel;

    [SerializeField]
    private TextMeshProUGUI otherInventoryTitle;

    [SerializeField]
    private ScrollRect otherInventoryScrollRect;

    [SerializeField]
    private RectTransform otherInventoryViewportRectTransport;

    [SerializeField]
    private GridLayoutGroup otherInventoryGridLayoutGroup;

    [HeaderAttribute("Inventory Controls")]
    [SerializeField]
    private Transform inventoryControlPanelTransform;

    [SerializeField]
    private TextMeshProUGUI inventoryControlTextPrefab;

    [HeaderAttribute("Dialogs")]
    [SerializeField]
    private TransferDialog transferDialog;

    [SerializeField]
    private ConfirmationDialog confirmationDialog;

    private int inventoryGridWidth = TWO_COLUMN_GRID_WIDTH;
    private Inventory playerInventory, otherInventory;

    private InputAction closeAction;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction transferAction;
    private InputAction transferXAction;
    private InputAction dropAction;

    private List<ItemStackUI> playerItemStackUIs;
    private List<ItemStackUI> otherItemStackUIs;

    private ItemStackUI selectedItemStackUI;

    private bool isOpen;
    private bool isDialogOpen;
    private bool isOtherInventorySelected;

    private bool allowInput { get { return isOpen && !isDialogOpen; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton InventoryUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // get player input actions
        closeAction = playerInput.actions["Close"];
        upAction = playerInput.actions["Up"];
        downAction = playerInput.actions["Down"];
        leftAction = playerInput.actions["Left"];
        rightAction = playerInput.actions["Right"];
        transferAction = playerInput.actions["Transfer"];
        transferXAction = playerInput.actions["Transfer X"];

        // bind player input action handlers
        upAction.started += _ => { if (allowInput) { NavigateInventoryUp(); } };
        downAction.started += _ => { if (allowInput) { NavigateInventoryDown(); } };
        leftAction.started += _ => { if (allowInput) { NavigateInventoryLeft(); } };
        rightAction.started += _ => { if (allowInput) { NavigateInventoryRight(); } };

        closeAction.started += _ => { if (allowInput) { CloseInventory(); } };
        transferAction.started += _ => { if (allowInput) { TransferStack(); } };
        transferXAction.started += _ => { if (allowInput) { TransferStackX(); } };
        dropAction.started += _ => { if (allowInput) { DropStack(); } };
    }

    /// <summary>
    /// Opens the inventory UI. If otherInventory is not null the interface will show both
    /// inventories and allow the player to transfer items between the two inventories.
    /// </summary>
    /// <param name="playerInventory">The player's inventory.</param>
    /// <param name="otherInventory">Another inventory that the player is accessing.</param>
    public void OpenInventory(Inventory playerInventory, Inventory otherInventory = null)
    {
        // set the inventories that are being accessed
        this.playerInventory = playerInventory;
        this.otherInventory = otherInventory;

        if (otherInventory != null)
        {
            // if there we are accessing two inventories, initialize a 3-column layout where column
            // 1 is the player's inventory, column 2 is the selected item description, and column 3
            // is the other inventory that we are accessing
            inventoryGridWidth = THREE_COLUMN_GRID_WIDTH;
            playerInventoryGridLayoutGroup.constraintCount = inventoryGridWidth;
            otherInventoryGridLayoutGroup.constraintCount = inventoryGridWidth;
            UpdateInventoryGrid();
            otherInventoryPanel.SetActive(true);
        }
        else
        {
            // if we are only accessing the player inventory, initialize a 2-column layout where
            // column 1 is the player inventory and column 2 is the selected item description
            inventoryGridWidth = TWO_COLUMN_GRID_WIDTH;
            playerInventoryGridLayoutGroup.constraintCount = inventoryGridWidth;
            UpdateInventoryGrid();
            otherInventoryPanel.SetActive(false);
        }

        // select the first item in the player's inventory and open the inventory UI
        SelectItemStack(playerItemStackUIs[0], true);
        playerInventoryGridLayoutGroup.GetComponent<RectTransform>().localPosition = Vector2.zero;
        otherInventoryGridLayoutGroup.GetComponent<RectTransform>().localPosition = Vector2.zero;
        inventoryUIPanel.SetActive(true);
        isOpen = true;
        OnInventoryOpened?.Invoke();
    }

    /// <summary>
    /// Clears and repopulates the inventory grids. This function should be called whenever a change
    /// is made to the player inventory or the other inventory.
    /// </summary>
    public void UpdateInventoryGrid()
    {
        ClearInventoryGrid();
        playerItemStackUIs = PopulateInventoryGrid(playerInventoryGridLayoutGroup, playerInventory);
        string title = playerInventory.GetTitle();
        int count = playerInventory.GetItemStackCount();
        int max = playerInventory.GetMaxCapacity();
        playerInventoryTitle.text = $"{title} ({count}/{max})";
        if (otherInventory != null)
        {
            title = otherInventory.GetTitle();
            count = otherInventory.GetItemStackCount();
            max = otherInventory.GetMaxCapacity();
            otherItemStackUIs = PopulateInventoryGrid(otherInventoryGridLayoutGroup, otherInventory);
            otherInventoryTitle.text = $"{title} ({count}/{max})";
        }
    }

    private void ClearInventoryGrid()
    {
        // destroy all UI item stacks in the player inventory
        ItemStackUI[] itemStackUIs = playerInventoryGridLayoutGroup.GetComponentsInChildren<ItemStackUI>();
        foreach (ItemStackUI itemStackUI in itemStackUIs)
        {
            Destroy(itemStackUI.gameObject);
        }

        // destory all UI item stacks in the other inventory
        itemStackUIs = otherInventoryGridLayoutGroup.GetComponentsInChildren<ItemStackUI>();
        foreach (ItemStackUI itemStackUI in itemStackUIs)
        {
            Destroy(itemStackUI.gameObject);
        }

        // reset the arrays that store UI item stacks
        playerItemStackUIs = null;
        otherItemStackUIs = null;
    }

    private List<ItemStackUI> PopulateInventoryGrid(GridLayoutGroup gridLayoutGroup, Inventory inventory)
    {
        // populate the UI elements that represent each item stack in the specified inventory
        List<ItemStackUI> itemStackUIs = new List<ItemStackUI>();
        foreach (ItemStack itemStack in inventory.GetItems())
        {
            ItemStackUI itemStackUI = Instantiate<ItemStackUI>(itemStackUIPrefab);
            itemStackUI.SetItemStack(itemStack);
            itemStackUI.transform.SetParent(gridLayoutGroup.transform, false);
            itemStackUIs.Add(itemStackUI);
        }

        // populate empty UI item stacks for the remaining capacity in the inventory
        for (int i = 0; i < inventory.GetRemainingCapacity(); i++)
        {
            ItemStackUI itemStackUI = Instantiate<ItemStackUI>(itemStackUIPrefab);
            itemStackUI.SetItemStack(null);
            itemStackUI.transform.SetParent(gridLayoutGroup.transform, false);
            itemStackUIs.Add(itemStackUI);
        }

        // return an array representation of the created UI elements
        return itemStackUIs;
    }

    private void ResetScollView()
    {
        playerInventoryGridLayoutGroup.GetComponent<RectTransform>().localPosition = Vector2.zero;
        otherInventoryGridLayoutGroup.GetComponent<RectTransform>().localPosition = Vector2.zero;
    }

    private void SnapToSelectedItem()
    {
        Canvas.ForceUpdateCanvases();

        ScrollRect selectedScrollRect = playerInventoryScrollRect;

        RectTransform selectedItemRectTransform = selectedItemStackUI.GetComponent<RectTransform>();
        RectTransform selectedViewport = playerInventoryViewportRectTransform;

        RectTransform playerContentPanel = playerInventoryGridLayoutGroup.GetComponent<RectTransform>();
        RectTransform otherContentPanel = otherInventoryGridLayoutGroup.GetComponent<RectTransform>();

        GridLayoutGroup selectedGridLayoutGroup = playerInventoryGridLayoutGroup;
        RectTransform selectedContentPanel = playerContentPanel;
        if (isOtherInventorySelected)
        {
            selectedScrollRect = otherInventoryScrollRect;
            selectedViewport = otherInventoryViewportRectTransport;
            selectedGridLayoutGroup = otherInventoryGridLayoutGroup;
            selectedContentPanel = otherContentPanel;
        }

        // calculate viewport bounds
        float viewportHeight = selectedViewport.rect.height;
        float viewportTopY = selectedContentPanel.localPosition.y;
        float viewportBottomY = viewportTopY + viewportHeight;

        // calculate selected item bounds
        float selectedItemY = selectedItemRectTransform.localPosition.y * -1f;
        float selectedItemTopY = selectedItemY - selectedItemRectTransform.rect.height / 2;
        float selectedItemBottomY = selectedItemY + selectedItemRectTransform.rect.height / 2;

        if (selectedItemTopY < viewportTopY)
        {
            // if the item is above the viewport focus the top of the viewport on the selected item
            StopCoroutine("ScrollInventory");
            Vector2 targetLocalPosition = new Vector2(0f, selectedItemTopY);
            StartCoroutine(ScrollInventory(
                selectedContentPanel.localPosition,
                targetLocalPosition,
                0.1f));
        }

        if (selectedItemBottomY > viewportBottomY)
        {
            // if the item is below the viewport focus the bottom of the viewport on the selected item
            StopCoroutine("ScrollInventory");
            Vector2 targetLocalPosition = new Vector2(
                0f, selectedItemBottomY - viewportHeight);
            StartCoroutine(ScrollInventory(
                selectedContentPanel.localPosition,
                targetLocalPosition,
                0.1f));
        }
    }

    private void SetSelectedItemText()
    {
        if (selectedItemStackUI == null || selectedItemStackUI.GetItemStack() == null)
        {
            selectedItemTitle.text = "No Item Selected";
            selectedItemDescription.text = "";
        }
        else
        {
            selectedItemTitle.text = selectedItemStackUI.GetItemStack().GetItemName();
            selectedItemDescription.text = selectedItemStackUI.GetItemStack().GetItemDescription();
        }
    }

    private void AddInventoryControl(string control, string text)
    {
        TextMeshProUGUI closeText = Instantiate<TextMeshProUGUI>(inventoryControlTextPrefab);
        closeText.text = $"[{control}] {text}";
        closeText.transform.SetParent(inventoryControlPanelTransform);
    }

    private void SetInventoryControlsText()
    {
        foreach (TextMeshProUGUI controlText in inventoryControlPanelTransform.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(controlText.gameObject);
        }

        // always show the control for closing the inventory
        AddInventoryControl("Esc", "Close"); // TODO: cross-platform control hints

        bool isItemStackSelected = selectedItemStackUI.GetItemStack() != null;
        bool isTransferPossible = isItemStackSelected && otherInventory != null;

        if (isTransferPossible && selectedItemStackUI.GetItemStack().GetStackSize() > 1)
        {
            // if a stack containing multiple items is selected show the control for a partial transfer
            AddInventoryControl("Z", "Transfer X"); // TODO: cross-platform control hints
        }

        if (isTransferPossible)
        {
            // if an item stack is selected show the control for transferring the entire item stack
            AddInventoryControl("X", "Transfer"); // TODO: cross-platform control hints
        }

        if (isItemStackSelected && !isOtherInventorySelected)
        {
            // if an item stack in the player inventory is selected show the control for dropping the item stack
            AddInventoryControl("C", "Drop"); // TODO: cross-platform control hints
        }
    }

    private void SelectItemStack(ItemStackUI itemStackUI, bool isInitializing = false)
    {
        if (selectedItemStackUI != null)
        {
            // deselect the selected UI item stack
            selectedItemStackUI.Deselect();
        }

        // select the newly selected UI item stack
        selectedItemStackUI = itemStackUI;
        selectedItemStackUI.Select();

        // ensure the item is visible in the scroll view
        if (!isInitializing)
        {
            SnapToSelectedItem();
        }

        // update the selected item details text
        SetSelectedItemText();

        // update inventory controls
        SetInventoryControlsText();
    }

    private Vector2Int IndexToCoordinate(int index)
    {
        return new Vector2Int(
            index % inventoryGridWidth,
            index / inventoryGridWidth
        );
    }

    private int CoordinateToIndex(Vector2Int coordinate)
    {
        return coordinate.y * inventoryGridWidth + coordinate.x;
    }

    private ItemStackUI GetItemStackUIByCoordinate(Vector2Int coordinate)
    {
        // get the selected inventory
        List<ItemStackUI> selectedItemStacks = playerItemStackUIs;
        if (isOtherInventorySelected)
        {
            selectedItemStacks = otherItemStackUIs;
        }

        // check if the supplied coordinate is outside of the bounds of the selected inventory
        int index = CoordinateToIndex(coordinate);
        if (index >= selectedItemStacks.Count)
        {
            return null;
        }

        // return the specified item stack by index
        return selectedItemStacks[index];
    }

    private Vector2Int GetCoordinateByItemStackUI(ItemStackUI itemStackUI)
    {
        // if the supplied item stack UI element is null return the first item in the inventory
        if (itemStackUI == null)
        {
            return Vector2Int.zero;
        }

        // get the selected inventory
        List<ItemStackUI> selectedItemStackUIs = playerItemStackUIs;
        if (isOtherInventorySelected)
        {
            selectedItemStackUIs = otherItemStackUIs;
        }

        // search for the specified item stack UI element in the selected inventory
        for (int i = 0; i < selectedItemStackUIs.Count; i++)
        {
            if (selectedItemStackUIs[i] == itemStackUI)
            {
                return IndexToCoordinate(i);
            }
        }

        // if we failed to find the specified item stack UI element return the first item in the
        // inventory
        return Vector2Int.zero;
    }

    private void NavigateInventoryUp()
    {
        // get the coordinates for the selected item stack UI element
        Vector2Int selectedCoordinate = GetCoordinateByItemStackUI(selectedItemStackUI);

        if (selectedCoordinate.y > 0)
        {
            // if the selected item is not in the first row of the inventory select the item that is
            // directly above the selected item
            SelectItemStack(GetItemStackUIByCoordinate(new Vector2Int(
                selectedCoordinate.x,
                selectedCoordinate.y - 1
            )));
            return;
        }
        else
        {
            // if the selected item is in the top row of the inventory selected the furthest item
            // in the same column
            int maxInventoryY = (playerItemStackUIs.Count / inventoryGridWidth) + 1;
            for (int y = maxInventoryY; y >= 0; y--)
            {
                ItemStackUI itemStackUI = GetItemStackUIByCoordinate(new Vector2Int(
                    selectedCoordinate.x, y
                ));
                if (itemStackUI != null)
                {
                    SelectItemStack(itemStackUI);
                    return;
                }
            }
        }
    }

    private void NavigateInventoryDown()
    {
        // get the coordinates for the selected item stack UI element
        Vector2Int selectedCoordinate = GetCoordinateByItemStackUI(selectedItemStackUI);

        int maxInventoryY = playerItemStackUIs.Count / inventoryGridWidth;
        if (selectedCoordinate.y < maxInventoryY)
        {
            // if the selected item is not in the bottom inventory row select the top item in the
            // same column
            ItemStackUI itemStackUI = GetItemStackUIByCoordinate(new Vector2Int(
                selectedCoordinate.x,
                selectedCoordinate.y + 1
            ));
            if (itemStackUI != null)
            {
                SelectItemStack(itemStackUI);
                return;
            }
        }

        // if the selected item is in the bottom row of the inventory select the top item in the
        // same column
        SelectItemStack(GetItemStackUIByCoordinate(new Vector2Int(
            selectedCoordinate.x, 0
        )));
    }

    private void NavigateInventoryLeft()
    {
        // get the coordinates for the selected item stack UI element
        Vector2Int selectedCoordinate = GetCoordinateByItemStackUI(selectedItemStackUI);

        if (selectedCoordinate.x > 0)
        {
            // if the selected item is not in the first column of the inventory select the item that
            // that is to the left of the current item
            SelectItemStack(GetItemStackUIByCoordinate(new Vector2Int(
                selectedCoordinate.x - 1,
                selectedCoordinate.y
            )));
            return;
        }
        else
        {
            // if the selected item is in the leftmost column select the furthest item in the same
            // column
            if (!isOtherInventorySelected && otherInventory != null)
            {
                // try to select the furthest item from the other inventory
                isOtherInventorySelected = true;
                for (int x = inventoryGridWidth - 1; x >= 0; x--)
                {
                    ItemStackUI itemStackUI = GetItemStackUIByCoordinate(new Vector2Int(
                        x, selectedCoordinate.y
                    ));
                    if (itemStackUI != null)
                    {
                        SelectItemStack(itemStackUI);
                        return;
                    }
                }
            }

            // try to select the furthest item from the player inventory
            isOtherInventorySelected = false;
            for (int x = inventoryGridWidth - 1; x >= 0; x--)
            {
                ItemStackUI itemStackUI = GetItemStackUIByCoordinate(new Vector2Int(
                    x, selectedCoordinate.y
                ));
                if (itemStackUI != null)
                {
                    SelectItemStack(itemStackUI);
                    return;
                }
            }
        }
    }

    private void NavigateInventoryRight()
    {
        // get the coordinates for the selected item stack UI element
        Vector2Int selectedCoordinate = GetCoordinateByItemStackUI(selectedItemStackUI);

        if (selectedCoordinate.x < inventoryGridWidth - 1)
        {
            // if the selected item is not in the last column of the inventory select the item that
            // that is to the right of the current item
            ItemStackUI itemStackUI = GetItemStackUIByCoordinate(new Vector2Int(
                selectedCoordinate.x + 1,
                selectedCoordinate.y
            ));
            if (itemStackUI != null)
            {
                SelectItemStack(itemStackUI);
                return;
            }
        }

        if (!isOtherInventorySelected && otherInventory != null)
        {
            // try to select an item from the other inventory
            isOtherInventorySelected = true;
            ItemStackUI itemStackUI = GetItemStackUIByCoordinate(new Vector2Int(
                0, selectedCoordinate.y
            ));
            if (itemStackUI != null)
            {
                SelectItemStack(itemStackUI);
                return;
            }
        }

        isOtherInventorySelected = false;

        // if the selected item is in the rightmost column of the inventory select the furhtest
        // item in the same column
        SelectItemStack(GetItemStackUIByCoordinate(new Vector2Int(
            0, selectedCoordinate.y
        )));
    }

    private void TransferStack()
    {
        if (otherInventory == null)
        {
            // if there is no other inventory don't allow item stack transfers
            return;
        }

        Inventory selected = playerInventory;
        Inventory to = otherInventory;
        if (isOtherInventorySelected)
        {
            selected = otherInventory;
            to = playerInventory;
        }

        if (selectedItemStackUI.GetItemStack() == null)
        {
            return;
        }

        Vector2Int selectedPosition = GetCoordinateByItemStackUI(selectedItemStackUI);
        ItemStack itemStack = selectedItemStackUI.GetItemStack();
        selected.TransferItem(itemStack, to, itemStack.GetStackSize());
        UpdateInventoryGrid();
        StartCoroutine(SelectPositionNextFrame(selectedPosition));
    }

    private void TransferStackX()
    {
        if (otherInventory == null)
        {
            // if there is no other inventory don't allow item stack transfers
            return;
        }

        Inventory selected = playerInventory;
        Inventory to = otherInventory;
        if (isOtherInventorySelected)
        {
            selected = otherInventory;
            to = playerInventory;
        }

        if (selectedItemStackUI.GetItemStack() == null)
        {
            return;
        }

        Vector2Int selectedPosition = GetCoordinateByItemStackUI(selectedItemStackUI);
        ItemStack selectedItemStack = selectedItemStackUI.GetItemStack();
        transferDialog.Initialize(selected, to, selectedItemStack);
        transferDialog.gameObject.SetActive(true);
        isDialogOpen = true;
        transferDialog.OnTransferSelected += TransferDialog_OnTransferSelected;
        transferDialog.OnTransferCancelled += TransferDialog_OnTransferCancelled;
    }

    private void DropStack()
    {
        Inventory selected = playerInventory;
        if (isOtherInventorySelected)
        {
            selected = otherInventory;
        }

        if (selectedItemStackUI.GetItemStack() == null)
        {
            return;
        }

        Vector2Int selectedPosition = GetCoordinateByItemStackUI(selectedItemStackUI);
        ItemStack selectedItemStack = selectedItemStackUI.GetItemStack();
        string itemName = selectedItemStack.GetItemName();
        int stackSize = selectedItemStack.GetStackSize();
        confirmationDialog.Initialize($"Drop {itemName} ({stackSize})?", () =>
        {
            selected.RemoveItemStack(selectedItemStack);
        });
        confirmationDialog.gameObject.SetActive(true);
        isDialogOpen = true;
        confirmationDialog.OnConfirmationComplete += ConfirmationDialog_OnConfirmationComplete;
    }

    private void CloseInventory()
    {
        ClearInventoryGrid();
        ResetScollView();
        isOtherInventorySelected = false;
        inventoryUIPanel.SetActive(false);
        isOpen = false;
        OnInventoryClosed?.Invoke();
    }

    private void CloseTransferDialog()
    {
        transferDialog.gameObject.SetActive(false);
        isDialogOpen = false;
    }

    private void CloseConfirmationDialog()
    {
        confirmationDialog.gameObject.SetActive(false);
        isDialogOpen = false;
    }

    private IEnumerator SelectPositionNextFrame(Vector2Int position)
    {
        yield return new WaitForEndOfFrame();
        SelectItemStack(GetItemStackUIByCoordinate(position));
    }

    private IEnumerator ScrollInventory(Vector3 initialLocalPosition, Vector3 targetLocalPosition, float duration)
    {
        RectTransform playerContentPanel = playerInventoryGridLayoutGroup.GetComponent<RectTransform>();
        RectTransform otherContentPanel = otherInventoryGridLayoutGroup.GetComponent<RectTransform>();

        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            Vector3 localPosition = Vector3.Lerp(
                initialLocalPosition,
                targetLocalPosition,
                time / duration);
            playerContentPanel.localPosition = localPosition;
            otherContentPanel.localPosition = localPosition;
            yield return null;
        }

        playerContentPanel.localPosition = targetLocalPosition;
        otherContentPanel.localPosition = targetLocalPosition;
    }

    private void TransferDialog_OnTransferSelected(
        Inventory from,
        Inventory to,
        ItemStack itemStack,
        int amount)
    {
        from.TransferItem(itemStack, to, amount);
        CloseTransferDialog();
    }

    private void TransferDialog_OnTransferCancelled()
    {
        CloseTransferDialog();
    }

    private void ConfirmationDialog_OnConfirmationComplete()
    {
        CloseConfirmationDialog();
    }
}
