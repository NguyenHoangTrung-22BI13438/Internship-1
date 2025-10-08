using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ShopController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI coinsText;
    public Transform itemsContainer;
    public GameObject shopItemUIPrefab;
    public Button backButton;

    [Header("Shop Items")]
    public List<ShopItem> availableItems;

    private int playerCoins;

    void Start()
    {
        playerCoins = ParametersScript.scoreValue;
        UpdateCoinsDisplay();
        PopulateShop();
        backButton.onClick.AddListener(() => SceneManager.LoadScene("Level01"));
    }

    void PopulateShop()
    {
        foreach (var item in availableItems)
        {
            var ui = Instantiate(shopItemUIPrefab, itemsContainer);
            ui.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
            ui.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = item.itemName;
            ui.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = item.price.ToString();
            ui.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = item.value.ToString();

            var btn = ui.transform.Find("BuyButton").GetComponent<Button>();
            btn.onClick.AddListener(() => TryBuy(item, ui));
            btn.interactable = playerCoins >= item.price;
        }
    }

    void TryBuy(ShopItem item, GameObject ui)
    {
        if (playerCoins < item.price) return;
        playerCoins -= item.price;
        ParametersScript.scoreValue = playerCoins;
        ApplyItemEffect(item);
        UpdateCoinsDisplay();
        ui.transform.Find("BuyButton").GetComponent<Button>().interactable = false;
    }

    void ApplyItemEffect(ShopItem item)
    {
        switch (item.type)
        {
            case ShopItem.ShopItemType.HealthPotion:
                ParametersScript.healValue = Mathf.Min(1000, ParametersScript.healValue + item.value);
                break;
            case ShopItem.ShopItemType.DamageUpgrade:
                PlayerPrefs.SetInt("DamageUpgrade", PlayerPrefs.GetInt("DamageUpgrade", 0) + item.value);
                break;
            case ShopItem.ShopItemType.SpeedUpgrade:
                PlayerPrefs.SetFloat("SpeedUpgrade", PlayerPrefs.GetFloat("SpeedUpgrade", 1f) + item.value);
                break;
            case ShopItem.ShopItemType.Weapon:
                // instantiate weapon or add to inventory
                break;
        }
    }

    void UpdateCoinsDisplay()
    {
        coinsText.text = $"Coins: {playerCoins}";
    }
}
