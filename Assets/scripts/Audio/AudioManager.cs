using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource soundFXPrefab;

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
        if (spawn == null)
        spawn = transform; // Default to this object's transform
        AudioSource audioSource = Instantiate(soundFXPrefab, spawn.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
