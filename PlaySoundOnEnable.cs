using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    private AudioSource audioSource;

    private void OnEnable()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && audioSource.clip != null)
            audioSource.Play();
    }
}