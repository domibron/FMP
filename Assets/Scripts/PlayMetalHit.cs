using UnityEngine;

public class PlayMetalHit : MonoBehaviour, IBulletHit
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip[] clips;

    public void Hit(Vector3 pos)
    {
        audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
    }
}
