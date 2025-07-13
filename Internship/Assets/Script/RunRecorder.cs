using System.Collections.Generic;
using UnityEngine;

public class RunRecorder : MonoBehaviour
{
    public static PlayerData lastRunData;

    public static void SavePlayerData(Player player)
    {
        BaseWeapon weapon = player.weapon?.GetComponent<BaseWeapon>();

        lastRunData = new PlayerData
        {
            maxHealth = ParametersScript.healValue,
            speed = 5f, // Or get from player if you add a movement speed field
            acquiredSkills = new List<string>(), // Not implemented yet
            acquiredItems = new List<string>(),  // Not implemented yet

            weaponName = weapon?.name,
            weaponDamage = weapon?.damage ?? 0,
            weaponFireRate = (weapon is Gun gun) ? gun.fireRate : 0,
            spriteName = player.GetComponent<SpriteRenderer>()?.sprite?.name

        };
    }
}
