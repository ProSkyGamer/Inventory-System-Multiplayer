using System;
using Unity.Netcode;
using UnityEngine;

public class InventoryObject : NetworkBehaviour
{
    #region Enums

    public enum ItemRarity
    {
        Common,
        Rare,
        Epic
    }

    #endregion

    #region Events

    public static event EventHandler OnInventoryParentChanged;

    public event EventHandler OnObjectRepaired;

    #endregion

    #region Variables & References

    [SerializeField] private Sprite inventoryObjectSprite;
    [SerializeField] private Sprite brokenInventoryObjectSprite;
    [SerializeField] private TextTranslationsSO inventoryObjectNameTextTranslationSo;
    [SerializeField] private ItemRarity itemRarity = ItemRarity.Common;

    [SerializeField] private WeaponSO weaponSo;
    [SerializeField] private RelicSO relicSo;
    [SerializeField] private bool isBroken;

    private IInventoryParent inventoryObjectParent;

    #endregion

    #region Inventory Object Initialization

    public void SpawnInventoryObject()
    {
        SpawnInventoryObjectServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnInventoryObjectServerRpc()
    {
        SpawnInventoryObjectClientRpc();
    }

    [ClientRpc]
    private void SpawnInventoryObjectClientRpc()
    {
        if (weaponSo != null)
        {
            var newWeapon = ScriptableObject.CreateInstance<WeaponSO>();
            newWeapon.SetWeaponsSo(weaponSo);
            weaponSo = newWeapon;
        }

        if (relicSo != null)
        {
            var newRelic = ScriptableObject.CreateInstance<RelicSO>();

            newRelic.SetRelicSo(relicSo);
            relicSo = newRelic;
        }
    }

    #endregion

    #region Inventory Parent

    public void SetInventoryParent(NetworkObjectReference inventoryNetworkObjectReference,
        CharacterInventoryUI.InventoryType inventoryType, bool isNeedToSendNotification = false)
    {
        SetInventoryParentServerRpc(inventoryNetworkObjectReference, (int)inventoryType, isNeedToSendNotification);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetInventoryParentServerRpc(NetworkObjectReference inventoryObjectOwnerNetworkObjectReference,
        int intInventoryType, bool isNeedToSendNotification)
    {
        SetInventoryParentClientRpc(inventoryObjectOwnerNetworkObjectReference, intInventoryType,
            isNeedToSendNotification);
    }

    [ClientRpc]
    private void SetInventoryParentClientRpc(NetworkObjectReference inventoryObjectOwnerNetworkObjectReference,
        int intInventoryType, bool isNeedToSendNotification)
    {
        inventoryObjectOwnerNetworkObjectReference.TryGet(out var inventoryObjectOwnerNetworkObject);

        IInventoryParent storedInventory;
        switch ((CharacterInventoryUI.InventoryType)intInventoryType)
        {
            default:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerInventory>();
                break;
            case CharacterInventoryUI.InventoryType.PlayerInventory:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerInventory>();
                break;
            case CharacterInventoryUI.InventoryType.PlayerRelicsInventory:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerRelics>();
                break;
            case CharacterInventoryUI.InventoryType.PlayerWeaponInventory:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerWeapons>();
                break;
        }

        inventoryObjectParent?.RemoveInventoryObjectBySlot(inventoryObjectParent.GetSlotNumberByInventoryObject(this));
        storedInventory.AddInventoryObject(this, isNeedToSendNotification);
        inventoryObjectParent = storedInventory;

        OnInventoryParentChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetInventoryParentBySlot(NetworkObjectReference inventoryObjectOwnerNetworkObjectReference,
        int intInventoryType, int slotNumber, bool isNeedToSendNotification = false)
    {
        SetInventoryParentBySlotServerRpc(inventoryObjectOwnerNetworkObjectReference, intInventoryType, slotNumber,
            isNeedToSendNotification);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetInventoryParentBySlotServerRpc(NetworkObjectReference inventoryObjectOwnerNetworkObjectReference,
        int intInventoryType, int newSlotNumber, bool isNeedToSendNotification)
    {
        SetInventoryParentBySlotClientRpc(inventoryObjectOwnerNetworkObjectReference, intInventoryType, newSlotNumber,
            isNeedToSendNotification);
    }

    [ClientRpc]
    private void SetInventoryParentBySlotClientRpc(NetworkObjectReference inventoryObjectOwnerNetworkObjectReference,
        int intInventoryType, int newSlotNumber, bool isNeedToSendNotification)
    {
        inventoryObjectOwnerNetworkObjectReference.TryGet(out var inventoryObjectOwnerNetworkObject);

        IInventoryParent storedInventory;
        switch ((CharacterInventoryUI.InventoryType)intInventoryType)
        {
            default:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerInventory>();
                break;
            case CharacterInventoryUI.InventoryType.PlayerInventory:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerInventory>();
                break;
            case CharacterInventoryUI.InventoryType.PlayerRelicsInventory:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerRelics>();
                break;
            case CharacterInventoryUI.InventoryType.PlayerWeaponInventory:
                storedInventory = inventoryObjectOwnerNetworkObject.GetComponent<PlayerWeapons>();
                break;
        }

        if (!storedInventory.IsSlotNumberAvailable(newSlotNumber)) return;

        inventoryObjectParent?.RemoveInventoryObjectBySlot(
            inventoryObjectParent.GetSlotNumberByInventoryObject(this));

        storedInventory.AddInventoryObjectToSlot(this, newSlotNumber, isNeedToSendNotification);
        inventoryObjectParent = storedInventory;

        OnInventoryParentChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveInventoryParent()
    {
        RemoveInventoryParentServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RemoveInventoryParentServerRpc()
    {
        RemoveInventoryParentClientRpc();
    }

    [ClientRpc]
    private void RemoveInventoryParentClientRpc()
    {
        inventoryObjectParent?.RemoveInventoryObjectBySlot(inventoryObjectParent.GetSlotNumberByInventoryObject(this));
        inventoryObjectParent = null;

        OnInventoryParentChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Drop To World

    public void DropInventoryObjectToWorld(Vector3 dropPosition)
    {
        DropInventoryObjectToWorldServerRpc(dropPosition);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropInventoryObjectToWorldServerRpc(Vector3 dropPosition)
    {
        dropPosition += new Vector3(0f, 0.5f, 0f);
        DroppedItemsController.Instance.AddNewDroppedItem(dropPosition, out var droppedItemNetworkObjectReference);
        RemoveInventoryParent();

        DropInventoryObjectToWorldClientRpc(droppedItemNetworkObjectReference);
    }

    [ClientRpc]
    private void DropInventoryObjectToWorldClientRpc(NetworkObjectReference droppedItemNetworkObjectReference)
    {
        DroppedItemsController.Instance.SetDroppedItem(droppedItemNetworkObjectReference, this);
    }

    #endregion

    #region Get Inventory Object Data

    public IInventoryParent GetInventoryObjectParent()
    {
        return inventoryObjectParent;
    }

    public Sprite GetInventoryObjectSprite()
    {
        return isBroken ? brokenInventoryObjectSprite : inventoryObjectSprite;
    }

    public ItemRarity GetItemRarity()
    {
        return itemRarity;
    }

    public TextTranslationsSO GetInventoryObjectNameTextTranslationSo()
    {
        return inventoryObjectNameTextTranslationSo;
    }

    public bool IsBroken()
    {
        return isBroken;
    }

    public bool TryGetWeaponSo(out WeaponSO gottenWeaponSo)
    {
        gottenWeaponSo = weaponSo;
        return gottenWeaponSo != null;
    }

    public bool TryGetRelicSo(out RelicSO gottenRelicSo)
    {
        gottenRelicSo = relicSo;
        return gottenRelicSo != null;
    }

    #endregion

    #region Inventory Object Break States

    public void BreakObject()
    {
        if (!IsServer) return;

        ChangeBrokenObjectStateServerRpc(true);
    }

    public void RepairObject()
    {
        if (!IsServer) return;

        ChangeBrokenObjectStateServerRpc(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeBrokenObjectStateServerRpc(bool newState)
    {
        ChangeBrokenObjectStateServerRpcClientRpc(newState);
    }

    [ClientRpc]
    private void ChangeBrokenObjectStateServerRpcClientRpc(bool newState)
    {
        isBroken = newState;

        if (!isBroken)
            OnObjectRepaired?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Relic Usages Reset

    public void TryNullifyRelicUsages()
    {
        if (!IsServer) return;
        if (relicSo == null) return;

        TryNullifyRelicUsagesServerRpc();
    }

    [ServerRpc]
    private void TryNullifyRelicUsagesServerRpc()
    {
        var newRelicUsagesCount = 0;

        TryNullifyRelicUsagesClientRpc(newRelicUsagesCount);
    }

    [ClientRpc]
    private void TryNullifyRelicUsagesClientRpc(int newRelicUsagesCount)
    {
        foreach (var relicApplyingEffect in relicSo.relicApplyingEffects)
        {
            if (!relicApplyingEffect.isUsagesLimited) continue;

            relicApplyingEffect.currentUsages = newRelicUsagesCount;
        }
    }

    #endregion

    #region Destoroy Inventory Object

    public void DestroyInventoryObject()
    {
        DestroyInventoryObjectServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyInventoryObjectServerRpc()
    {
        var inventoryNetworkObject = GetComponent<NetworkObject>();

        inventoryNetworkObject.Despawn();
        Destroy(gameObject);
    }

    #endregion
}
