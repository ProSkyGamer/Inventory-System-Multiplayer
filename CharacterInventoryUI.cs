using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventoryUI : NetworkBehaviour
{
    #region Events

    public static event EventHandler OnAnySlotInteractButtonPressed;
    public static event EventHandler OnStopItemDragging;

    #endregion

    #region Enums

    public enum InventoryType
    {
        PlayerInventory,
        PlayerWeaponInventory,
        PlayerRelicsInventory
    }

    public enum InventoryItemDescriptionSize
    {
        Small,
        Medium,
        Large
    }

    public enum InventoryItemInteractButtonsSize
    {
        Small,
        Medium,
        Large
    }

    #endregion

    #region Variables & References

    [SerializeField] private InventoryType inventoryType = InventoryType.PlayerInventory;

    [SerializeField] private Transform playerInventorySlotPrefab;
    [SerializeField] private Transform playerInventorySlotsGrid;
    [SerializeField] private bool isInventoryInteractable = true;
    [SerializeField] private bool isShowingItemsDescription = true;

    [SerializeField] private InventoryItemDescriptionSize inventoryItemDescriptionSize =
        InventoryItemDescriptionSize.Large;

    [SerializeField] private bool isShowingInteractButtons = true;

    [SerializeField] private InventoryItemInteractButtonsSize inventoryItemInteractButtonsSize =
        InventoryItemInteractButtonsSize.Large;

    [SerializeField] private bool isShowingObjectName = true;

    [SerializeField] private Image currentDraggingImage;
    private Transform currentSlotDescription;
    private Transform currentSlotInteractButtons;
    private static InventoryObject currentDraggingObject;
    private static CharacterInventoryUI currentDraggingObjectPreviousInventoryParentUI;
    private static int currentDraggingObjectPreviousInventoryParentSlotNumber;

    private readonly List<InventorySlotSingleUI> allInventorySlots = new();

    private bool isFirstUpdate;

    private IInventoryParent followingInventory;

    #endregion

    #region Inititalization & Subscribed events

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        PlayerController.OnPlayerSpawned += PlayerController_OnPlayerSpawned;

        OnAnySlotInteractButtonPressed += CharacterInventoryUI_OnAnySlotInteractButtonPressed;
        InventorySlotSingleUI.OnStartItemDragging += InventorySlotSingleUI_OnStartItemDragging;

        InventorySlotSingleUI.OnDisplaySlotDescription += InventorySlotSingleUI_OnDisplaySlotDescription;
        InventorySlotSingleUI.OnStopDisplaySlotDescription += InventorySlotSingleUI_OnStopDisplaySlotDescription;
        CharacterUI.OnCharacterUIClose += InventorySlotSingleUI_OnStopDisplaySlotDescription;

        InventorySlotSingleUI.OnDisplaySlotInteractButtons += InventorySlotSingleUI_OnDisplaySlotInteractButtons;

        InventoryObject.OnInventoryParentChanged += InventoryObject_OnInventoryParentChanged;
    }

    private void InventoryObject_OnInventoryParentChanged(object sender, EventArgs e)
    {
        if (currentSlotInteractButtons != null)
        {
            Destroy(currentSlotInteractButtons.gameObject);
            currentSlotInteractButtons = null;
        }

        UpdateInventory();
    }

    private void PlayerController_OnPlayerSpawned(object sender, EventArgs e)
    {
        followingInventory = GetCurrentPlayerInventoryByType();

        followingInventory.OnInventorySizeChanged += FollowingInventory_OnInventorySizeChanged;

        isFirstUpdate = true;

        PlayerController.OnPlayerSpawned -= PlayerController_OnPlayerSpawned;
    }

    private void FollowingInventory_OnInventorySizeChanged(object sender, EventArgs e)
    {
        UpdateInventory();
    }

    private void CharacterInventoryUI_OnAnySlotInteractButtonPressed(object sender, EventArgs e)
    {
        if (currentSlotInteractButtons != null)
        {
            Destroy(currentSlotInteractButtons.gameObject);
            currentSlotInteractButtons = null;
        }

        UpdateInventory();
    }

    private void InventorySlotSingleUI_OnDisplaySlotInteractButtons(object sender,
        InventorySlotSingleUI.OnDisplaySlotInteractButtonsEventArgs e)
    {
        if (!isInventoryInteractable) return;
        if (!isShowingInteractButtons) return;

        if (currentSlotInteractButtons != null)
        {
            Destroy(currentSlotInteractButtons.gameObject);
            currentSlotInteractButtons = null;
        }

        if (e.displayedInventory != this) return;

        var slotInteractButtonsPrefab =
            GetAdditionalUIPrefabs.Instance.GetInventoryItemInteractButtonPrefabBySize(
                inventoryItemInteractButtonsSize);

        var newSlotInteractButton = Instantiate(slotInteractButtonsPrefab,
            GameInput.Instance.GetCurrentMousePosition(),
            Quaternion.identity, transform.GetComponentsInParent<Transform>()[1]);

        var slotInteractButtonsUI = newSlotInteractButton.GetComponent<InventorySlotInteractButtons>();
        slotInteractButtonsUI.SetSlotInfo(e.inventoryObject, inventoryType,
            () => { OnAnySlotInteractButtonPressed?.Invoke(this, EventArgs.Empty); });
        currentSlotInteractButtons = newSlotInteractButton;
    }

    private void InventorySlotSingleUI_OnStopDisplaySlotDescription(object sender, EventArgs e)
    {
        if (currentSlotDescription != null)
        {
            Destroy(currentSlotDescription.gameObject);
            currentSlotDescription = null;
        }
    }

    private void InventorySlotSingleUI_OnDisplaySlotDescription(object sender,
        InventorySlotSingleUI.OnDisplaySlotDescriptionEventArgs e)
    {
        if (!isShowingItemsDescription) return;

        if (currentSlotInteractButtons != null)
        {
            Destroy(currentSlotInteractButtons.gameObject);
            currentSlotInteractButtons = null;
        }

        if (e.displayedInventory != this) return;

        var slotDescriptionPrefab =
            GetAdditionalUIPrefabs.Instance.GetInventoryItemDescriptionPrefabBySize(
                inventoryItemDescriptionSize);

        var newSlotDescription = Instantiate(slotDescriptionPrefab,
            GameInput.Instance.GetCurrentMousePosition(), Quaternion.identity,
            transform.GetComponentsInParent<Transform>()[1]);
        var inventoryItemSlotDescription = newSlotDescription.GetComponent<InventoryItemDescription>();
        inventoryItemSlotDescription.SetInventoryObject(e.inventoryObject);

        currentSlotDescription = newSlotDescription;
    }

    private void InventorySlotSingleUI_OnStartItemDragging(object sender,
        InventorySlotSingleUI.OnStartItemDraggingEventArgs e)
    {
        if (!isInventoryInteractable) return;

        currentDraggingObject = e.draggingInventoryObject;
        currentDraggingObjectPreviousInventoryParentUI = e.previousInventoryParent;
        currentDraggingObjectPreviousInventoryParentSlotNumber = e.previousInventorySlotNumber;

        currentDraggingImage.gameObject.SetActive(true);
        currentDraggingImage.sprite = currentDraggingObject.GetInventoryObjectSprite();
    }

    #endregion

    #region Update

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            UpdateInventorySlotCount();
        }

        if (currentDraggingObject == null) return;

        currentDraggingImage.transform.position = GameInput.Instance.GetCurrentMousePosition();

        if (GameInput.Instance.GetBindingValue(GameInput.Binding.UpgradesStartDragging) != 1f)
        {
            var selectedSlot = GetCurrentSelectedSlot();

            if (selectedSlot == null) return;

            var inventoryToPlaceNetworkObjectReference =
                new NetworkObjectReference(PlayerController.Instance.GetPlayerNetworkObject());

            var selectedSlotStoredItem = selectedSlot.GetStoredItem();
            if (selectedSlotStoredItem != null)
            {
                var previousSlot = currentDraggingObjectPreviousInventoryParentUI.GetInventorySlotSingleUIBySlotNumber(
                    currentDraggingObjectPreviousInventoryParentSlotNumber);

                previousSlot.StoreItem(selectedSlotStoredItem);

                currentDraggingObject.RemoveInventoryParent();
                selectedSlotStoredItem.SetInventoryParentBySlot(inventoryToPlaceNetworkObjectReference,
                    (int)currentDraggingObjectPreviousInventoryParentUI.inventoryType,
                    currentDraggingObjectPreviousInventoryParentSlotNumber);
            }

            selectedSlot.StoreItem(currentDraggingObject);

            var newSlotNumber = selectedSlot.GetSlotNumber();

            currentDraggingObject.SetInventoryParentBySlot(inventoryToPlaceNetworkObjectReference, (int)inventoryType,
                newSlotNumber);

            currentDraggingImage.gameObject.SetActive(false);

            currentDraggingObject = null;

            OnStopItemDragging?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion

    #region Updating Inventory

    private void UpdateInventorySlotCount()
    {
        var maxSlotCount = followingInventory.GetMaxSlotsCount();

        if (allInventorySlots.Count == maxSlotCount) return;

        if (allInventorySlots.Count < maxSlotCount)
        {
            playerInventorySlotPrefab.gameObject.SetActive(true);
            currentDraggingImage.gameObject.SetActive(true);

            for (var i = allInventorySlots.Count; i < maxSlotCount; i++)
            {
                var slotTransform = Instantiate(playerInventorySlotPrefab, playerInventorySlotsGrid);

                var slotSingleUI = slotTransform.GetComponent<InventorySlotSingleUI>();
                slotSingleUI.SetStarterData(i, inventoryType, isInventoryInteractable, isShowingObjectName, this);

                allInventorySlots.Add(slotSingleUI);
            }

            playerInventorySlotPrefab.gameObject.SetActive(false);
            currentDraggingImage.gameObject.SetActive(false);
        }
        else
        {
            for (var i = 0; i < allInventorySlots.Count - maxSlotCount; i++)
            {
                allInventorySlots.RemoveAt(i);
                i--;
            }
        }
    }

    public void UpdateInventory()
    {
        UpdateInventorySlotCount();

        currentDraggingImage.gameObject.SetActive(false);

        var playerInventory = GetCurrentPlayerInventoryByType();
        for (var i = 0; i < allInventorySlots.Count; i++)
        {
            var inventoryObject = playerInventory.GetInventoryObjectBySlot(i);

            if (inventoryObject == null)
            {
                allInventorySlots[i].RemoveItem();
                continue;
            }

            allInventorySlots[i].StoreItem(inventoryObject);
        }
    }

    #endregion

    #region Following Inventory

    public void ChangeFollowingInventory(IInventoryParent newFollowingInventory)
    {
        followingInventory.OnInventorySizeChanged -= FollowingInventory_OnInventorySizeChanged;

        followingInventory = newFollowingInventory;
        isInventoryInteractable = false;

        followingInventory.OnInventorySizeChanged += FollowingInventory_OnInventorySizeChanged;
    }

    #endregion

    #region Get Inventory Data

    public IInventoryParent GetCurrentPlayerInventoryByType()
    {
        if (followingInventory != null) return followingInventory;

        switch (inventoryType)
        {
            default:
                return PlayerController.Instance.GetPlayerInventory();
            case InventoryType.PlayerInventory:
                return PlayerController.Instance.GetPlayerInventory();
            case InventoryType.PlayerWeaponInventory:
                return PlayerController.Instance.GetPlayerWeaponsInventory();
            case InventoryType.PlayerRelicsInventory:
                return PlayerController.Instance.GetPlayerRelicsInventory();
        }
    }

    private InventorySlotSingleUI GetCurrentSelectedSlot()
    {
        foreach (var inventorySlot in allInventorySlots)
            if (inventorySlot.IsCurrentSlotSelected())
                return inventorySlot;

        return null;
    }

    public static InventoryObject GetCurrentDraggingObject()
    {
        return currentDraggingObject;
    }

    public InventorySlotSingleUI GetInventorySlotSingleUIBySlotNumber(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= allInventorySlots.Count) return null;

        return allInventorySlots[slotNumber];
    }

    #endregion

    public static void ResetStaticData()
    {
        OnStopItemDragging = null;
        OnAnySlotInteractButtonPressed = null;
    }
}
