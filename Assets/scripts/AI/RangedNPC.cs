using UnityEngine;

public class RangedNPC : NPC
{
    private GameObject target;
    public AudioClip shot;

    [Header("Parameters for ranged combat")]
    public float safeDistance = 5f; // Minimum safe distance from the enemy
    public GameObject projectilePrefab;      
    public Transform projectileSpawnPoint;

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
            // AudioManager.Instance.PlaySound(shot);

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
