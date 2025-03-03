using UnityEngine;

public class RangedNPC : NPC
{
    private GameObject target;

    [Header("Parameters for ranged combat")]
    public float safeDistance = 5f; // Minimum safe distance from the enemy
    public GameObject projectilePrefab;      
    public Transform projectileSpawnPoint;  

    void Update()
    {
        if (target == null)
        target = FindNearestEnemy();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            LookAt(target.transform.position);

            if (distance < safeDistance)
            {
                // If the enemy is too close, we retreat (move in the opposite direction)
                Vector3 retreatDirection = (transform.position - target.transform.position).normalized;
                transform.position += retreatDirection * moveSpeed * Time.deltaTime;
            }
            else if (distance <= attackRange)
            {
                // If the enemy is in the attack zone and is at a safe distance, we attack
                if (Time.time >= lastAttackTime + attackDelay)
                {
                    Attack(target);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                // If the enemy is too far away, we approach, but keep our distance.
                Vector3 approachDirection = (target.transform.position - transform.position).normalized;
                transform.position += approachDirection * moveSpeed * Time.deltaTime;
            }
        }
    }

    protected override void Attack(GameObject target)
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Projectile projectile = proj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetStats(target.transform, damage);
            }
        }
        else
        {
            Debug.LogWarning("The projectile prefab or spawn point is not assigned " + gameObject.name);
        }
    }
}
