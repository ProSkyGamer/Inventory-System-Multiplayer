using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItemDescription : MonoBehaviour
{
    #region Variables & References

    [SerializeField] private TextTranslationSingleUI inventoryObjectName;
    [SerializeField] private TextTranslationSingleUI inventoryObjectType;
    [SerializeField] private Transform inventoryObjectRarityGrid;
    [SerializeField] private Transform inventoryObjectRaritySingleStar;

    [SerializeField] private Transform weaponInfoTransform;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponTypeText;
    [SerializeField] private TextTranslationsSO inventoryObjectWeaponTypeTextTranslationSo;
    [SerializeField] private TextTranslationSingleUI inventoryObjectWeaponType;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponAdditionalStatTypeText;
    [SerializeField] private TextTranslationsSO inventoryObjectWeaponAdditionalStatTypeTextTranslationSo;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponAdditionalStat;
    [SerializeField] private TextTranslationsSO inventoryObjectWeaponPassiveTextTranslationSo;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponPassiveText;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponPassive;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponScalesText;
    [SerializeField] private TextTranslationsSO inventoryObjectWeaponScalesTextTranslationSo;
    [SerializeField] private TextTranslationsSO inventoryObjectWeaponNormalAttackScalesTextTranslationsSo;
    [SerializeField] private TextTranslationsSO inventoryObjectWeaponChargedAttackScaleTextTranslationsSo;
    [SerializeField] private TextMeshProUGUI inventoryObjectWeaponAttackScales;

    [SerializeField] private Transform relicInfoTransform;
    [SerializeField] private TextMeshProUGUI inventoryObjectRelicWhileEquippedText;
    [SerializeField] private TextTranslationsSO inventoryObjectRelicWhileEquippedTextTranslationSo;
    [SerializeField] private TextMeshProUGUI inventoryObjectRelicPassive;
    [SerializeField] private TextTranslationsSO inventoryObjectRelicUsagesLeftTextTranslationsSo;
    [SerializeField] private TextMeshProUGUI inventoryObjectRelicUsagesLeft;

    [SerializeField] private Transform brokenStateTransform;
    [SerializeField] private TextTranslationsSO brokenTextTranslationsSo;
    [SerializeField] private TextMeshProUGUI brokenText;

    #endregion

    #region Inventory Object Methods

    public void SetInventoryObject(InventoryObject inventoryObject)
    {
        inventoryObjectName.ChangeTextTranslationSO(inventoryObject.GetInventoryObjectNameTextTranslationSo());

        inventoryObjectType.ChangeTextTranslationSO(
            GetAdditionalInventoryTextTranslationSo.Instance.GetObjectTypeTextTranslationSoByInventoryObject(
                inventoryObject));

        var inventoryObjectRarity = inventoryObject.GetItemRarity();
        var starsToAdd = 0;
        switch (inventoryObjectRarity)
        {
            case InventoryObject.ItemRarity.Rare:
                starsToAdd = 1;
                break;
            case InventoryObject.ItemRarity.Epic:
                starsToAdd = 2;
                break;
        }

        for (var i = 0; i < starsToAdd; i++) Instantiate(inventoryObjectRaritySingleStar, inventoryObjectRarityGrid);

        if (!inventoryObject.IsBroken())
            brokenStateTransform.gameObject.SetActive(false);
        else
            brokenText.text =
                TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                    TextTranslationController.GetCurrentLanguage(), brokenTextTranslationsSo);

        if (!inventoryObject.TryGetWeaponSo(out var weaponSo))
        {
            weaponInfoTransform.gameObject.SetActive(false);
        }
        else
        {
            inventoryObjectWeaponTypeText.text =
                TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                    TextTranslationController.GetCurrentLanguage(), inventoryObjectWeaponTypeTextTranslationSo);
            inventoryObjectWeaponAdditionalStatTypeText.text =
                TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                    TextTranslationController.GetCurrentLanguage(),
                    inventoryObjectWeaponAdditionalStatTypeTextTranslationSo);
            inventoryObjectWeaponScalesText.text =
                TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                    TextTranslationController.GetCurrentLanguage(), inventoryObjectWeaponScalesTextTranslationSo);
            inventoryObjectWeaponPassiveText.text =
                TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                    TextTranslationController.GetCurrentLanguage(), inventoryObjectWeaponPassiveTextTranslationSo);

            inventoryObjectWeaponType.ChangeTextTranslationSO(GetAdditionalInventoryTextTranslationSo.Instance
                .GetWeaponTypeTextTranslationSoByInventoryObject(inventoryObject));
            var additionalStatNameText = TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(),
                GetAdditionalInventoryTextTranslationSo.Instance
                    .GetWeaponAdditionalStatTypeTextTranslationSoByInventoryObject(inventoryObject));
            inventoryObjectWeaponAdditionalStat.text = $"<align=left> {additionalStatNameText} </align> " +
                                                       $"<align=right> {weaponSo.additionalWeaponStatTypeScale * 100}% </align>";

            inventoryObjectWeaponPassive.text = GetEffectsTextFromEffectList(weaponSo.weaponPassiveTalent, out var _);

            var singleNormalAttackWeaponScaleText = TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(),
                inventoryObjectWeaponNormalAttackScalesTextTranslationsSo);
            var singleChargeAttackWeaponScaleText = TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(),
                inventoryObjectWeaponChargedAttackScaleTextTranslationsSo);
            var displayedString = "";
            for (var i = 0; i < weaponSo.comboAttack; i++)
            {
                displayedString += string.Format(singleNormalAttackWeaponScaleText, i + 1,
                    weaponSo.comboAttackScales[i] * 100);
                displayedString += "\n";
            }

            displayedString += "\n";
            displayedString +=
                string.Format(singleChargeAttackWeaponScaleText, weaponSo.chargedAttackDamageScale * 100);

            inventoryObjectWeaponAttackScales.text = displayedString;
        }

        if (!inventoryObject.TryGetRelicSo(out var relicSo))
        {
            relicInfoTransform.gameObject.SetActive(false);
        }
        else
        {
            inventoryObjectRelicWhileEquippedText.text =
                TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                    TextTranslationController.GetCurrentLanguage(), inventoryObjectRelicWhileEquippedTextTranslationSo);

            inventoryObjectRelicPassive.text =
                GetEffectsTextFromEffectList(relicSo.relicApplyingEffects, out var effectLimitString);
            if (effectLimitString == "")
                inventoryObjectRelicUsagesLeft.gameObject.SetActive(false);

            inventoryObjectRelicUsagesLeft.text = effectLimitString;
        }
    }

    #endregion

    #region Get Additional Text

    private string GetEffectsTextFromEffectList(List<PlayerEffectsController.AppliedEffect> givenEffects,
        out string fullEffectLimitString)
    {
        var fullEffectString = "";
        fullEffectLimitString = "";

        foreach (var effect in givenEffects)
        {
            var fullEffectConditionString =
                GetAdditionalInventoryTextTranslationSo.Instance.GetEffectConditionTextTranslationByEffect(effect);

            fullEffectString += string.Format(fullEffectConditionString,
                effect.effectCondition is { isConditionValueFlat: true }
                    ? effect.effectCondition.conditionValue
                    : effect.effectCondition.conditionValue * 100,
                effect.effectCondition is { isStepSizeFlat: true }
                    ? effect.effectCondition.stepSize
                    : effect.effectCondition.stepSize * 100);

            var singleEffectString = TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(),
                GetAdditionalInventoryTextTranslationSo.Instance.GetPlayerEffectTextTranslationSoByEffect(
                    effect));

            fullEffectString += string.Format(singleEffectString, effect.effectPercentageScale * 100,
                effect.effectApplyingChance, effect.effectLimit, effect.applyingEffectDuration);
            fullEffectString += "\n";

            if (effect.isUsagesLimited)
                fullEffectLimitString = string.Format(
                    TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                        TextTranslationController.GetCurrentLanguage(),
                        inventoryObjectRelicUsagesLeftTextTranslationsSo),
                    effect.maxUsagesLimit - effect.currentUsages);
        }

        return fullEffectString;
    }

    #endregion
}
