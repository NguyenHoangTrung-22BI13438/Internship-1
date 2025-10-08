using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public string description;
    public Sprite itemIcon;
    public int price;
    public ShopItemType itemType;
    public int value; // For health potions, damage upgrades, etc.

    public enum ShopItemType
    {
        HealthPotion,
        DamageUpgrade,
        SpeedUpgrade,
        Weapon
    }
}
