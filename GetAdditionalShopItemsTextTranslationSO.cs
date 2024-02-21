using UnityEngine;

public class GetAdditionalShopItemsTextTranslationSO : MonoBehaviour
{
    #region Variables & References

    [Header("Selling Item Type's")]
    [SerializeField] private TextTranslationsSO noTypeTextTranslationSo;

    [SerializeField] private TextTranslationsSO experienceTextTranslationSo;
    [SerializeField] private TextTranslationsSO levelTextTranslationSo;
    [SerializeField] private TextTranslationsSO relicTextTranslationSo;
    [SerializeField] private TextTranslationsSO relicResetTextTranslationSo;

    #endregion

    public static GetAdditionalShopItemsTextTranslationSO Instance { get; private set; }

    #region Initialization

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #endregion

    #region Get Shop Items TranslationsSO

    public TextTranslationsSO GetShopItemTypeTextTranslationSoByShopItem(ShopItem shopItem)
    {
        switch (shopItem.soldShopItemType)
        {
            default:
                return noTypeTextTranslationSo;
            case ShopItem.ShopItemType.Experience:
                return experienceTextTranslationSo;
            case ShopItem.ShopItemType.Level:
                return levelTextTranslationSo;
            case ShopItem.ShopItemType.Relic:
                return relicTextTranslationSo;
            case ShopItem.ShopItemType.RelicReset:
                return relicResetTextTranslationSo;
        }
    }

    #endregion
}
