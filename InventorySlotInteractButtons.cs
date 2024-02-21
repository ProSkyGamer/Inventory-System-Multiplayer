using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotInteractButtons : MonoBehaviour
{
    #region Variables & References

    [SerializeField] private Button unEquipItem;
    [SerializeField] private Button equipItem;
    [SerializeField] private Button dropItem;
    [SerializeField] private Button deleteItem;

    [SerializeField] private TextMeshProUGUI unEquipItemText;
    [SerializeField] private TextTranslationsSO unEquipItemTextTranslationsSo;
    [SerializeField] private TextMeshProUGUI equipItemText;
    [SerializeField] private TextTranslationsSO equipItemTextTranslationsSo;
    [SerializeField] private TextMeshProUGUI dropItemText;
    [SerializeField] private TextTranslationsSO dropItemTextTranslationsSo;
    [SerializeField] private TextMeshProUGUI deleteItemText;
    [SerializeField] private TextTranslationsSO deleteItemTextTranslationsSo;

    #endregion

    #region Slot Buttons Methods

    public void SetSlotInfo(InventoryObject inventoryObject,
        CharacterInventoryUI.InventoryType storedInventoryType, Action onClickAction)
    {
        deleteItemText.text =
            TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(), deleteItemTextTranslationsSo);
        dropItemText.text = TextTranslationController.GetTextFromTextTranslationSOByLanguage(
            TextTranslationController.GetCurrentLanguage(), dropItemTextTranslationsSo);

        deleteItem.onClick.AddListener(() =>
        {
            inventoryObject.RemoveInventoryParent();
            inventoryObject.DestroyInventoryObject();
            onClickAction();
        });

        dropItem.onClick.AddListener(() =>
        {
            inventoryObject.DropInventoryObjectToWorld(PlayerController.Instance.transform.position);
            onClickAction();
        });

        if (storedInventoryType == CharacterInventoryUI.InventoryType.PlayerWeaponInventory &&
            PlayerController.Instance.GetPlayerWeaponsInventory().GetCurrentInventoryObjectsCount() <= 1)
        {
            unEquipItem.interactable = false;
            equipItem.interactable = false;
            dropItem.interactable = false;
            deleteItem.interactable = false;
        }

        IInventoryParent inventoryReference = null;
        var inventoryNetworkObjectReference =
            new NetworkObjectReference(PlayerController.Instance.GetPlayerNetworkObject());
        var inventoryType = CharacterInventoryUI.InventoryType.PlayerInventory;
        if (storedInventoryType == CharacterInventoryUI.InventoryType.PlayerInventory)
        {
            equipItemText.text = TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(), equipItemTextTranslationsSo);

            unEquipItem.gameObject.SetActive(false);


            if (inventoryObject.TryGetWeaponSo(out var _))
            {
                inventoryType = CharacterInventoryUI.InventoryType.PlayerWeaponInventory;
                inventoryReference = PlayerController.Instance.GetPlayerWeaponsInventory();
            }
            else if (inventoryObject.TryGetRelicSo(out var _))
            {
                inventoryType = CharacterInventoryUI.InventoryType.PlayerRelicsInventory;
                inventoryReference = PlayerController.Instance.GetPlayerRelicsInventory();
            }

            if (inventoryReference != null)
            {
                if (inventoryReference.IsHasAnyAvailableSlot())
                    equipItem.onClick.AddListener(() =>
                    {
                        inventoryObject.SetInventoryParent(inventoryNetworkObjectReference, inventoryType);
                        onClickAction();
                    });

                return;
            }

            equipItem.interactable = false;
        }
        else
        {
            unEquipItemText.text = TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(), unEquipItemTextTranslationsSo);

            equipItem.gameObject.SetActive(false);

            inventoryReference = PlayerController.Instance.GetPlayerInventory();
            if (inventoryReference.IsHasAnyAvailableSlot())
            {
                unEquipItem.onClick.AddListener(() =>
                {
                    inventoryObject.SetInventoryParent(inventoryNetworkObjectReference, inventoryType);
                    onClickAction();
                });
                return;
            }


            unEquipItem.interactable = false;
        }
    }

    #endregion
}
