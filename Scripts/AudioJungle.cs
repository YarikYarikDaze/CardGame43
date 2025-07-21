using UnityEngine;
using UnityEngine.Audio;

public class AudioJungle : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource[] sources;
    void Awake()
    {
        sources = new AudioSource[clips.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].clip = clips[i];
        }
    }
    public void PlayClip(int index)
    {
        sources[index].Play();
    }
}
