using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;

    private float damage;
    private Transform target;

    public void SetStats(Transform targetTransform, float dam = 10f)
    {
        target = targetTransform;
        damage = dam;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            target.GetComponent<Damageable>().Damage(damage);
            Destroy(gameObject);
        }
    }
}

