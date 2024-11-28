using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading new scenes
        }
        else
        {
            Destroy(gameObject);// If an instance already exists, destroy this one
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audioClip, float soundVolume) //play a specific audio
    {
        audioSource.PlayOneShot(audioClip, soundVolume);
    }
}
