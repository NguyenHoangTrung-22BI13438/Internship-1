using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public GameObject weapon;

    float count = 0;

    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        this.followMouse();
    }

    private void followMouse()
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        spriteRenderer.flipX = direction.x < 0;
    }
    public void TakeDamage(float amount)
    {
        ParametersScript.healValue -= (int)amount;

        if (ParametersScript.healValue <= 0)
        {
            // ✅ Save the player's run data before death
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr && sr.sprite != null)
            {
                RunRecorder.lastRunData = new PlayerData
                {
                    maxHealth = ParametersScript.healValue,
                    speed = 5f, // you can change this if you store player speed elsewhere
                    acquiredSkills = new List<string>(), // not yet implemented
                    acquiredItems = new List<string>(),  // not yet implemented
                    weaponName = weapon?.name,
                    weaponDamage = weapon?.GetComponent<BaseWeapon>()?.damage ?? 0,
                    weaponFireRate = (weapon?.GetComponent<Gun>()?.fireRate) ?? 0,
                    spriteName = sr.sprite.name // ✅ Save sprite name for mirror boss
                };
            }

            // ✅ Reset health and score
            int score = PlayerPrefs.GetInt("score", 0);
            int heal = PlayerPrefs.GetInt("heal", 1000);
            ParametersScript.healValue = heal;
            ParametersScript.scoreValue = score;

            // ✅ Return to main menu scene (assumed build index 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("xyz: " + other.collider.tag + " - " + ParametersScript.healValue);
        switch (other.collider.tag)
        {
            case TAG.ENEMY:
                TakeDamage(100);
                break;
            case TAG.ENEMY_BULLET:
                TakeDamage(200);
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        count += Time.deltaTime;
        if (count > 1)
        {
            switch (other.collider.tag)
            {
                case TAG.ENEMY:
                    TakeDamage(50);
                    break;
                case TAG.ENEMY_BULLET:
                    TakeDamage(100);
                    break;
            }
            count = 0;
        }
    }
}
