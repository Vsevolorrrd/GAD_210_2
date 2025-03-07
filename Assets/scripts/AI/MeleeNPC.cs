using UnityEngine;

public class MeleeNPC : NPC
{
    private GameObject target;
    [SerializeField] Animator anim;

    [Header("Search for enemy")]
    [SerializeField] private float checkInterval = 5f;
    private float checkTimer = 0f;

    [Header("Fall")]
    public float fallThresholdY = -100f;
    void Update()
    {
        if (transform.position.y < fallThresholdY)
        Die(); // kills enemy if it falls of map

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
                    if(anim)
                    anim.SetTrigger("Attack");
                    Invoke("AttackDelay", 0.3f);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
    private void AttackDelay() // so that attack is in sync with animation
    {
        Attack(target);
    }
}
