using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public float maxHealth;
    public float speed;
    public List<string> acquiredSkills;
    public List<string> acquiredItems;

    public string weaponName;      // Prefab name
    public float weaponDamage;
    public float weaponFireRate;   // 0 if sword

    public string spriteName; // The name of the player's sprite

}
