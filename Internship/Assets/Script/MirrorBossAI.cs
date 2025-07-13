using UnityEngine;

public class MirrorBossAI : BaseEnemy
{
    private BaseWeapon weapon;
    private float attackTimer = 0f;
    private Animator anim;

    public float detectionRange = 10f;

    protected override void Start()
    {
        base.Start();
        weapon = GetComponentInChildren<BaseWeapon>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (target == null) target = FindObjectOfType<Player>();
        if (target == null || weapon == null) return;

        Vector2 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;

        // Flip sprite to face player
        transform.localScale = new Vector3(dir.x < 0 ? -1 : 1, 1, 1);

        float moveSpeed = 0f;

        if (!(weapon is Gun))
        {
            if (distance > 1.5f)
            {
                moveSpeed = 2f;
                transform.position += (Vector3)(dir.normalized * moveSpeed * Time.deltaTime);
            }
        }

        // Sync animator with movement
        if (anim != null)
        {
            anim.SetFloat("move", moveSpeed);
        }

        // Auto-attack
        attackTimer += Time.deltaTime;

        if (weapon is Gun gun && attackTimer * gun.fireRate >= 1f)
        {
            attackTimer = 0f;
            var fireMethod = weapon.GetType().GetMethod("Fire", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            fireMethod?.Invoke(weapon, null);
        }
        else if (weapon.name.ToLower().Contains("sword"))
        {
            var atk = weapon.GetComponentInChildren<SwordAttack>();
            if (atk != null && !atk.attacking)
            {
                atk.SendMessage("StartAttack", SendMessageOptions.DontRequireReceiver);

                // Optional: trigger boss attack animation
                // anim?.SetTrigger("Attack");
            }
        }
    }
}
