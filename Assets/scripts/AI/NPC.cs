using UnityEngine;

public enum Faction
{
    Faction1,
    Faction2,
    Faction3
}

public class NPC : Damageable
{
    [Header("Basic NPC parameters")]
    public Faction faction;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float attackDelay = 1f;     
    public float attackRange = 2f;     // Attack range (for close combat)
    public float damage = 20f;

    protected float lastAttackTime;

    // Search for the nearest enemy (an NPC belonging to another faction)
    protected GameObject FindNearestEnemy()
    {
        NPC[] allNPCs = FindObjectsOfType<NPC>();
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        foreach (NPC npc in allNPCs)
        {
            if (npc == this) continue;
            if (npc.faction == this.faction) continue;
            float distance = Vector3.Distance(transform.position, npc.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = npc.gameObject;
            }
        }
        return nearestEnemy;
    }

    // A virtual method of attack that can be redefined
    protected virtual void Attack(GameObject target)
    {
        Debug.Log($"{gameObject.name} attacks {target.name}");
        target.GetComponent<Damageable>().Damage(damage);
    }
    protected virtual void MoveTo(Vector3 pos)
    {

    }
    protected virtual void LookAt(Vector3 pos)
    {
        Vector3 direction = (pos - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
