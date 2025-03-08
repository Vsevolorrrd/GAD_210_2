using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource soundFXPrefab;
    [SerializeField] int maxSoundsPlaying = 25;
    private int currentSoundsPlaying = 0;

    private static AudioManager _instance;
    #region Singleton
    public static AudioManager Instance => _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("AudioManager already exists, destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion

    public void PlaySound(AudioClip audioClip, float volume = 1, Transform spawn = null)
    {
        if (currentSoundsPlaying >= maxSoundsPlaying)
        return;  // Do not play the sound if the limit is exceeded

        if (spawn == null)
        spawn = transform; // Default to this object's transform
        AudioSource audioSource = Instantiate(soundFXPrefab, spawn.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        currentSoundsPlaying++;

        float clipLength = audioSource.clip.length;
        StartCoroutine(DecreaseSoundCount(audioSource, clipLength));
    }

    private IEnumerator DecreaseSoundCount(AudioSource audioSource, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        currentSoundsPlaying--;  // Decrease the count when the sound is finished
        Destroy(audioSource.gameObject);
    }
}