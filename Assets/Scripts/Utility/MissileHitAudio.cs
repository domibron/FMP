using UnityEngine;

public class MissileHitAudio : MonoBehaviour, IMissileHit
{
    [SerializeField]
    PlayStopAudio playStopAudio;

    [SerializeField]
    CameraShake cameraShake;

    public void Hit(Vector3 position)
    {
        playStopAudio.Play();
        print("missile hit me");
        cameraShake?.ShakeCam(15f);
    }
}
