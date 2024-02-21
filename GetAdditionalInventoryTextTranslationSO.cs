using UnityEngine;

public class GetAdditionalInventoryTextTranslationSo : MonoBehaviour
{
    #region Variables & References

    [Header("Object Type's")]
    [SerializeField] private TextTranslationsSO noTypeTextTranslationSo;

    [SerializeField] private TextTranslationsSO relicTextTranslationSo;
    [SerializeField] private TextTranslationsSO weaponTextTranslationSo;

    [Header("Weapon Type's")]
    [SerializeField] private TextTranslationsSO noTypeWeaponTypeTextTranslationSo;

    [SerializeField] private TextTranslationsSO katanaWeaponTypeTextTranslationSo;
    [SerializeField] private TextTranslationsSO gunWeaponTypeTextTranslationSo;

    [Header("Weapon Additional Stats")]
    [SerializeField] private TextTranslationsSO noStatTypeTextTranslationSo;

    [SerializeField] private TextTranslationsSO atkTypeTextTranslationSo;
    [SerializeField] private TextTranslationsSO hpTypeTextTranslationSo;
    [SerializeField] private TextTranslationsSO defTypeTextTranslationSo;
    [SerializeField] private TextTranslationsSO critRateTypeTextTranslationSo;
    [SerializeField] private TextTranslationsSO critDmgTextTypeTranslationSo;
    [SerializeField] private TextTranslationsSO naDmgBonusTextTypeTranslationSo;
    [SerializeField] private TextTranslationsSO caDmgBonusTypeTextTranslationSo;

    [Header("Relic Buffs")]
    [SerializeField] private TextTranslationsSO noRelicBuffTextTranslationSo;

    [SerializeField] private TextTranslationsSO dmgAbsorptionEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO takenDmgIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO naDmgIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO caDmgIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO gainedExpIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO atkIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO defIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO hpIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO critRateIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO onHitCritRateIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO critDmgIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO deathSavingEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO movementSpeedIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO onEnemyDeathGainedCoinsIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO staminaConsumptionDecreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO skillPointsExpRequirementDecreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO playerInventorySlotsIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO weaponInventorySlotsIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO relicInventorySlotsIncreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO onHitEnemySpeedDecreaseEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO onHitEnemyPoisonEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO onHitEnemyStunEffectTextTranslationSo;
    [SerializeField] private TextTranslationsSO regenerationPerKillEffectTextTranslationSo;

    [Header("Effect Conditions")]
    [SerializeField] private TextTranslationsSO effectConditionStringStart;

    [SerializeField] private TextTranslationsSO effectConditionTypeHpMoreThen;
    [SerializeField] private TextTranslationsSO effectConditionTypeHpLessThen;
    [SerializeField] private TextTranslationsSO effectConditionTypeDefMoreThen;
    [SerializeField] private TextTranslationsSO effectConditionTypeDefLessThen;
    [SerializeField] private TextTranslationsSO effectConditionIfEndless;

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

    public static GetAdditionalInventoryTextTranslationSo Instance { get; private set; }

    #region Get TransaltionSO Data

    public TextTranslationsSO GetObjectTypeTextTranslationSoByInventoryObject(InventoryObject inventoryObject)
    {
        if (inventoryObject.TryGetRelicSo(out var _))
            return relicTextTranslationSo;
        if (inventoryObject.TryGetWeaponSo(out var _))
            return weaponTextTranslationSo;

        return noTypeTextTranslationSo;
    }

    public TextTranslationsSO GetWeaponTypeTextTranslationSoByInventoryObject(InventoryObject inventoryObject)
    {
        if (!inventoryObject.TryGetWeaponSo(out var weaponSo)) return noTypeWeaponTypeTextTranslationSo;

        switch (weaponSo.weaponType)
        {
            default:
                return noTypeWeaponTypeTextTranslationSo;
            case WeaponSO.WeaponType.Katana:
                return katanaWeaponTypeTextTranslationSo;
            case WeaponSO.WeaponType.Gun:
                return gunWeaponTypeTextTranslationSo;
        }
    }

    public TextTranslationsSO GetPlayerEffectTextTranslationSoByEffect(PlayerEffectsController.AppliedEffect effect)
    {
        switch (effect.appliedEffectType)
        {
            default:
                return noRelicBuffTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.TakenDmgDecrease:
                return dmgAbsorptionEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.TakenDmgIncrease:
                return takenDmgIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.NADmgIncrease:
                return naDmgIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.CADmgIncrease:
                return caDmgIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.GainedExpIncrease:
                return gainedExpIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.DefIncrease:
                return defIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.HpIncrease:
                return hpIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.HpRegenerationPerKill:
                return regenerationPerKillEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.AtkIncrease:
                return atkIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.CritRateIncrease:
                return critRateIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.OnHitEnemyCritRateIncrease:
                return onHitCritRateIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.CritDamageIncrease:
                return critDmgIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.DeathSaving:
                return deathSavingEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.MovementSpeedIncrease:
                return movementSpeedIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.SkillPointExpRequirementDecrease:
                return skillPointsExpRequirementDecreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.PlayerInventorySlotsIncrease:
                return playerInventorySlotsIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.WeaponInventorySlotsIncrease:
                return weaponInventorySlotsIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.RelicInventorySlotsIncrease:
                return relicInventorySlotsIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.OnHitEnemySpeedDecrease:
                return onHitEnemySpeedDecreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.OnHitEnemyPoison:
                return onHitEnemyPoisonEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.OnHitEnemyStun:
                return onHitEnemyStunEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.GainedCoinsOnEnemyDeathIncrease:
                return onEnemyDeathGainedCoinsIncreaseEffectTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.StaminaConsumptionDecrease:
                return staminaConsumptionDecreaseEffectTextTranslationSo;
        }
    }

    public TextTranslationsSO GetWeaponAdditionalStatTypeTextTranslationSoByInventoryObject(
        InventoryObject inventoryObject)
    {
        if (!inventoryObject.TryGetWeaponSo(out var weaponSo)) return noStatTypeTextTranslationSo;

        switch (weaponSo.additionalWeaponStatType)
        {
            default:
                return noStatTypeTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.AtkIncrease:
                return atkTypeTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.HpIncrease:
                return hpTypeTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.DefIncrease:
                return defTypeTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.CritRateIncrease:
                return critRateTypeTextTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.CritDamageIncrease:
                return critDmgTextTypeTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.NADmgIncrease:
                return naDmgBonusTextTypeTranslationSo;
            case PlayerEffectsController.AllPlayerEffects.CADmgIncrease:
                return caDmgBonusTypeTextTranslationSo;
        }
    }

    #endregion

    #region Get Effect Condition Description

    public string GetEffectConditionTextTranslationByEffect(PlayerEffectsController.AppliedEffect effect)
    {
        var fullConditionString = "";

        if (effect.effectCondition.conditionValue != 0f)
        {
            fullConditionString += TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                TextTranslationController.GetCurrentLanguage(), effectConditionStringStart);
            switch (effect.effectCondition.conditionType)
            {
                case PlayerEffectsController.AppliedEffect.EffectCondition.ConditionType.CurrentHpHigherThen:
                    fullConditionString += TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                        TextTranslationController.GetCurrentLanguage(), effectConditionTypeHpMoreThen);
                    break;
                case PlayerEffectsController.AppliedEffect.EffectCondition.ConditionType.CurrentHpLowerThen:
                    fullConditionString += TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                        TextTranslationController.GetCurrentLanguage(), effectConditionTypeHpLessThen);
                    break;
                case PlayerEffectsController.AppliedEffect.EffectCondition.ConditionType.DefHigherThen:
                    fullConditionString += TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                        TextTranslationController.GetCurrentLanguage(), effectConditionTypeDefMoreThen);
                    break;
                case PlayerEffectsController.AppliedEffect.EffectCondition.ConditionType.DefLowerThen:
                    fullConditionString += TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                        TextTranslationController.GetCurrentLanguage(), effectConditionTypeDefLessThen);
                    break;
            }

            fullConditionString += " {0}";
            if (!effect.effectCondition.isConditionValueFlat)
                fullConditionString += "%";

            if (effect.effectCondition.isCountEndless)
                fullConditionString += TextTranslationController.GetTextFromTextTranslationSOByLanguage(
                    TextTranslationController.GetCurrentLanguage(), effectConditionIfEndless);

            fullConditionString += "\n";
        }

        return fullConditionString;
    }

    #endregion
}
