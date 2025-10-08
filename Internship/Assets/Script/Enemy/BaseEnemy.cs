using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    public Player target;

    public int maxHealth = 10;
    protected float _health = 0f;

    public Slider healthbar;

    public float health => _health;

    protected virtual void Start()
    {
        _health = maxHealth;
        if (healthbar != null)
        {
            healthbar.maxValue = maxHealth;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TAG.SWORD))
        {
            SwordAttack swordAttack = collision.GetComponent<SwordAttack>();
            if (swordAttack != null && swordAttack.attacking)
            {
                Debug.Log("Attacked by sword: " + swordAttack.baseWeapon.damage);
                swordAttack.attacking = false;
                GotDamage(swordAttack.baseWeapon.damage, collision);
            }
        }
        else if (collision.CompareTag(TAG.BULLET))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                Debug.Log("Attacked by bullet: " + bullet.GetDamage());
                GotDamage(bullet.GetDamage(), collision);
            }
        }
    }

    protected void GotDamage(float damage, Collider2D collider)
    {
        OnAttacked(collider);
        _health = Mathf.Max(0, _health - damage);
        if (_health <= 0)
        {
            OnDie();
        }
    }

    protected virtual void OnAttacked(Collider2D collider) { }

    protected virtual void OnDie()
    {
        ParametersScript.scoreValue += 10;
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        // 🧠 Fix: auto-find player if missing (support old scenes)
        if (target == null)
        {
            target = FindObjectOfType<Player>();
        }

        if (healthbar != null)
        {
            healthbar.value = this.health;
            healthbar.gameObject.SetActive(healthbar.value < maxHealth);
        }
    }
    public GameObject coinPrefab;  // assign in Inspector

    void Die()
    {
        if (coinPrefab != null)
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
