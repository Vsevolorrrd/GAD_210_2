using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damageable : MonoBehaviour
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] bool isVulnerable = true;
    private bool isDead = false;

    [Header("Visual Effects")]
    [SerializeField] bool flicker = true;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem damageEffect;
    [SerializeField] GameObject Remains;

    [Header("Audio")]
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;

    protected void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))// for testing
        {
            Damage(20);
        }
    }

    protected virtual void Initialize()
    {
        currentHealth = maxHealth;
        if (flicker)
        {
            if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (damageEffect)
        damageEffect.gameObject.SetActive(true);
    }

    public virtual void Damage(float damage)
    {
        if (isDead || !isVulnerable || damage <= 0)
        return;

        Debug.Log("damage");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (damageEffect)
        damageEffect.Play();
        if (flicker)
        StartCoroutine(Blink());
        if (damageSound)
        AudioManager.Instance.PlaySound(damageSound, 1f, transform);
    }

    public virtual void Heal(float amount)
    {
        if (isDead || amount <= 0)
        return;

        if (currentHealth + amount > maxHealth)
        currentHealth = maxHealth;
        else
        currentHealth += amount;
    }

    public virtual void Die()
    {
        isDead = true;
        if (deathSound)
        AudioManager.Instance.PlaySound(deathSound, 1f, transform);
        if (Remains != null)
        Instantiate(Remains, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    protected IEnumerator Blink()
    {
        if (meshRenderer == null || !meshRenderer.material.HasProperty("_Color"))
        yield break;

        Material mat = meshRenderer.material;
        Color originalColor = mat.color;

        // Ensure the start color is the original color
        mat.color = originalColor;

        float blinkDuration = 0.25f;
        float lerpTime = 0f;
        mat.color = Color.white * 2f;

        while (lerpTime < blinkDuration)
        {
            lerpTime += Time.deltaTime;
            mat.color = Color.Lerp(Color.white * 2f, originalColor, lerpTime / blinkDuration);
            yield return null;
        }

        // Ensure the final color is the original color
        mat.color = originalColor;
    }
}