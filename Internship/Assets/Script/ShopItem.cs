using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public Sprite icon;
    public int price;
    public ShopItemType type;
    public int value;  // effect magnitude

    public enum ShopItemType { HealthPotion, DamageUpgrade, SpeedUpgrade, Weapon }
}
