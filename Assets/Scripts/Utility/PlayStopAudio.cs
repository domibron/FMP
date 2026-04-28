using UnityEngine;
using UnityEngine.Audio;


public class PlayStopAudio : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (!audioSource.isPlaying) audioSource.Play();
    }

    public void PlayOneShot()
    {
        if (!audioSource.isPlaying) audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayScheduled(float time)
    {
        if (!audioSource.isPlaying) audioSource.PlayScheduled(time);
    }

    public void Stop()
    {
        if (audioSource.isPlaying) audioSource.Play();
    }
}
