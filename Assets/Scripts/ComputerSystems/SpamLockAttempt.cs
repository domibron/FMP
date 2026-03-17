using UnityEngine;

public class SpamLockAttempt : MonoBehaviour
{
    TrackingSystem trackingSystem;

    void Awake()
    {
        trackingSystem = GetComponent<TrackingSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        trackingSystem.TryLockTargetNearCenter();
    }
}
