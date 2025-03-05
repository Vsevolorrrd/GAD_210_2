using UnityEngine;

public class Giant : NPC
{
    private GameObject target;
    [SerializeField] Animator anim;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius = 5f;
    [SerializeField] float pushForce = 10f;
    [SerializeField] float pushUpwardModifier = 1f;
    [SerializeField] LayerMask enemyLayer;

    [Header("Search for enemy")]
    [SerializeField] private float checkInterval = 5f;
    private float checkTimer = 0f;

    void Update()
    {
        checkTimer += Time.deltaTime;

        if (target == null || checkTimer >= checkInterval)
        {
            target = FindNearestEnemy();
            checkTimer = 0f;
        }

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            LookAt(target.transform.position);

            if (distance > attackRange)
            {
                // Getting closer to the goal
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                // If the target is within range and the delay has expired, we will attack
                if (Time.time >= lastAttackTime + attackDelay)
                {
                    if (anim)
                    anim.SetTrigger("Attack");
                    Invoke("AttackDelay", 0.3f);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
    private void AreaAttack()
    {
        // Find all enemies in the attack radius
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayer);

        foreach (Collider collider in hitColliders)
        {
            GameObject enemy = collider.gameObject;
            if (enemy == gameObject) continue; // don't damage yourself

            Damageable damageable = enemy.GetComponent<Damageable>();
            if (damageable != null)
            damageable.Damage(damage);


            if (enemy.GetComponent<Giant>() != null)
            continue; // don't damage yourself

            // Apply knockback force if the enemy has a Rigidbody
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = (enemy.transform.position - transform.position).normalized;
                forceDirection.y += pushUpwardModifier; // Add an upward force
                rb.AddForce(forceDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
    private void AttackDelay() // so that attack is in sync with animation
    {
        AreaAttack();
    }
}