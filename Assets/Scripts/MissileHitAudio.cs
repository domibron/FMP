using UnityEngine;

public class MissileHitAudio : MonoBehaviour, IMissileHit
{
    [SerializeField]
    PlayStopAudio playStopAudio;

    public void Hit(Vector3 position)
    {
        playStopAudio.Play();
    }
}
