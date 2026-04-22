using UnityEngine;

public class PlayMetalHit : MonoBehaviour, IBulletHit
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip[] clips;

    [SerializeField]
    CameraShake cameraShake;

    public void Hit(Vector3 pos)
    {
        audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);

        cameraShake?.ShakeCam(5f);
    }
}
