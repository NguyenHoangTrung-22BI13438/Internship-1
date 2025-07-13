using UnityEngine;

public class MirrorBoss : BaseEnemy
{
    public GameObject gunPrefab;
    public GameObject swordPrefab;

    protected override void Start()
    {
        base.Start();
        InitializeMirrorData();
    }

    private void InitializeMirrorData()
    {
        if (RunRecorder.lastRunData == null)
        {
            Debug.LogWarning("No player data from last run.");
            return;
        }

        var data = RunRecorder.lastRunData;

        maxHealth = (int)data.maxHealth;
        _health = maxHealth;
        if (healthbar != null)
            healthbar.maxValue = maxHealth;

        GameObject weaponToUse = null;
        if (!string.IsNullOrEmpty(data.weaponName))
        {
            if (data.weaponName.ToLower().Contains("gun"))
                weaponToUse = Instantiate(gunPrefab, transform.position, Quaternion.identity);
            else if (data.weaponName.ToLower().Contains("sword"))
                weaponToUse = Instantiate(swordPrefab, transform.position, Quaternion.identity);
        }

        if (weaponToUse)
        {
            BaseWeapon weaponScript = weaponToUse.GetComponent<BaseWeapon>();
            weaponScript.damage = data.weaponDamage;

            if (weaponScript is Gun gun)
                gun.fireRate = data.weaponFireRate;

            weaponScript.parentEntity = this.gameObject;
        }

        if (!string.IsNullOrEmpty(data.spriteName))
        {
            Sprite newSprite = Resources.Load<Sprite>("Sprites/" + data.spriteName);
            if (newSprite)
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr) sr.sprite = newSprite;
            }
            else
            {
                Debug.LogWarning("MirrorBoss: Sprite not found in Resources/Sprites/ -> " + data.spriteName);
            }
        }
    }
}
