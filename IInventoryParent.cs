using System;

public interface IInventoryParent
{
    public event EventHandler OnInventorySizeChanged;

    public void AddInventoryObject(InventoryObject inventoryObject, bool isNeedToSendNotification);

    public void AddInventoryObjectToSlot(InventoryObject inventoryObject, int slotNumber,
        bool isNeedToSendNotification);

    public void RemoveInventoryObjectBySlot(int slotNumber);
    public void ChangeInventorySize(int newSize);
    public InventoryObject GetInventoryObjectBySlot(int slotNumber);
    public bool IsHasAnyInventoryObject();
    public bool IsSlotNumberAvailable(int slotNumber);
    public bool IsHasAnyAvailableSlot();
    public int GetFirstAvailableSlot();
    public int GetSlotNumberByInventoryObject(InventoryObject inventoryObject);
    public int GetMaxSlotsCount();
    public int GetCurrentInventoryObjectsCount();
}
