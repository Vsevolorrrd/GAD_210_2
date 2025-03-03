using UnityEngine;

public class MeleeNPC : NPC
{
    void Update()
    {
        GameObject target = FindNearestEnemy();
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
                    Attack(target);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}
