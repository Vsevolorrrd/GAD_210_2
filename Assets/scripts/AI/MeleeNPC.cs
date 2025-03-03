using UnityEngine;

public class MeleeNPC : NPC
{
    private GameObject target;
    [SerializeField] Animator anim;
    void Update()
    {
        if (target == null)
        target = FindNearestEnemy();

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
