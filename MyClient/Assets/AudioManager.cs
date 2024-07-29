using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource; // 전역적으로 사용할 AudioSource
    public AudioClip[] soundEffects; // AudioClip 배열 추가

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGlobalSound(AudioClip audioClip)
    {
        //if (audioSource != null)
        //{
        //    audioSource.PlayOneShot(audioClip);
        //}
        //else
        //{
        //    Debug.LogWarning("No global AudioSource found to play sound.");
        //}
    }
}