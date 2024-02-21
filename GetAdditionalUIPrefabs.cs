using UnityEngine;

public class GetAdditionalUIPrefabs : MonoBehaviour
{
    public static GetAdditionalUIPrefabs Instance { get; private set; }

    #region Variables & References

    [SerializeField] private Transform smallInventoryItemDescriptionPrefab;
    [SerializeField] private Transform mediumInventoryItemDescriptionPrefab;
    [SerializeField] private Transform largeInventoryItemDescriptionPrefab;

    [SerializeField] private Transform smallInventoryItemInteractButtonsPrefab;
    [SerializeField] private Transform mediumInventoryItemInteractButtonsPrefab;
    [SerializeField] private Transform largeInventoryItemInteractButtonsPrefab;

    [SerializeField] private Transform upgradesDescriptionPrefab;

    #endregion

    #region Initialization

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion

    #region Get Additional UI Prefabs

    public Transform GetInventoryItemDescriptionPrefabBySize(
        CharacterInventoryUI.InventoryItemDescriptionSize inventoryItemDescriptionSize)
    {
        switch (inventoryItemDescriptionSize)
        {
            default:
                return smallInventoryItemDescriptionPrefab;
            case CharacterInventoryUI.InventoryItemDescriptionSize.Small:
                return smallInventoryItemDescriptionPrefab;
            case CharacterInventoryUI.InventoryItemDescriptionSize.Medium:
                return mediumInventoryItemDescriptionPrefab;
            case CharacterInventoryUI.InventoryItemDescriptionSize.Large:
                return largeInventoryItemDescriptionPrefab;
        }
    }

    public Transform GetInventoryItemInteractButtonPrefabBySize(
        CharacterInventoryUI.InventoryItemInteractButtonsSize inventoryItemInteractButtonsSize)
    {
        switch (inventoryItemInteractButtonsSize)
        {
            default:
                return smallInventoryItemInteractButtonsPrefab;
            case CharacterInventoryUI.InventoryItemInteractButtonsSize.Small:
                return smallInventoryItemInteractButtonsPrefab;
            case CharacterInventoryUI.InventoryItemInteractButtonsSize.Medium:
                return mediumInventoryItemInteractButtonsPrefab;
            case CharacterInventoryUI.InventoryItemInteractButtonsSize.Large:
                return largeInventoryItemInteractButtonsPrefab;
        }
    }

    public Transform GetUpgradeDescriptionPrefab()
    {
        return upgradesDescriptionPrefab;
    }

    #endregion
}
