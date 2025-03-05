using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damageable : MonoBehaviour
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] bool isVulnerable = true;
    private bool isDead = false;
    private bool isBlinking = false; // Prevents multiple calls

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
        if (flicker && !isBlinking)
        StartCoroutine(Blink());
        if (damageSound)
        AudioManager.Instance.PlaySound(damageSound, 0.7f, transform);
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
        AudioManager.Instance.PlaySound(deathSound, 0.7f, transform);
        if (Remains != null)
        Instantiate(Remains, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    protected IEnumerator Blink()
    {
        if (meshRenderer == null || !meshRenderer.material.HasProperty("_Color"))
            yield break;

        isBlinking = true; // Prevents multiple calls

        Material mat = meshRenderer.material;
        Color originalColor = mat.color;
        Color flashColor = Color.white * 2f;

        float blinkDuration = 0.25f;
        float elapsedTime = 0f;

        mat.color = flashColor;

        while (elapsedTime < blinkDuration)
        {
            elapsedTime += Time.deltaTime;
            mat.color = Color.Lerp(flashColor, originalColor, elapsedTime / blinkDuration);
            yield return null;
        }

        mat.color = originalColor; // Ensure final reset
        isBlinking = false; // Allows new calls
    }
}