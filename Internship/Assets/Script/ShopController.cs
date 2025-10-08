using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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

        // Set up back button
        backButton.onClick.AddListener(GoBack);
    }

    void PopulateShop()
    {
        foreach (ShopItem item in availableItems)
        {
            GameObject itemUI = Instantiate(shopItemUIPrefab, itemsContainer);

            // Set up item UI components
            itemUI.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.itemIcon;
            itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.itemName;
            itemUI.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>().text = item.price.ToString();
            itemUI.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.description;

            Button buyButton = itemUI.transform.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(() => TryBuyItem(item));

            // Disable button if can't afford
            UpdateItemUI(itemUI, item);
        }
    }

    void UpdateItemUI(GameObject itemUI, ShopItem item)
    {
        Button buyButton = itemUI.transform.Find("BuyButton").GetComponent<Button>();
        buyButton.interactable = (playerCoins >= item.price);
    }

    void TryBuyItem(ShopItem item)
    {
        if (playerCoins >= item.price)
        {
            playerCoins -= item.price;
            ParametersScript.scoreValue = playerCoins;

            // Apply item effect
            ApplyItemEffect(item);

            UpdateCoinsDisplay();
            RefreshShopUI();

            Debug.Log($"Bought {item.itemName} for {item.price} coins!");
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    void ApplyItemEffect(ShopItem item)
    {
        switch (item.itemType)
        {
            case ShopItem.ShopItemType.HealthPotion:
                ParametersScript.healValue += item.value;
                if (ParametersScript.healValue > 1000) // Cap at max health
                    ParametersScript.healValue = 1000;
                break;

            case ShopItem.ShopItemType.DamageUpgrade:
                // You'll need to implement a damage upgrade system
                PlayerPrefs.SetInt("DamageUpgrade", PlayerPrefs.GetInt("DamageUpgrade", 0) + item.value);
                break;

            case ShopItem.ShopItemType.SpeedUpgrade:
                // You'll need to implement a speed upgrade system
                PlayerPrefs.SetInt("SpeedUpgrade", PlayerPrefs.GetInt("SpeedUpgrade", 0) + item.value);
                break;
        }
    }

    void UpdateCoinsDisplay()
    {
        coinsText.text = $"Coins: {playerCoins}";
    }

    void RefreshShopUI()
    {
        // Update all buy buttons based on current coins
        for (int i = 0; i < itemsContainer.childCount; i++)
        {
            GameObject itemUI = itemsContainer.GetChild(i).gameObject;
            ShopItem item = availableItems[i];
            UpdateItemUI(itemUI, item);
        }
    }

    void GoBack()
    {
        // Return to previous scene or main menu
        SceneManager.LoadScene("Level01"); // Adjust scene name as needed
    }
}
