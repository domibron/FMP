using UnityEngine;

public class TrackingData
{
    public Collider lockedTarget;
    public Collider[] storedDetectedTargets;
    public bool hasLock;
}

public class TrackingSystem : MonoBehaviour, IDataReadable
{
    [SerializeField]
    Radar radar;

    [SerializeField]
    Transform headOfPilot;

    Collider[] detectedEntities;

    Collider lockedTarget = null;

    bool hasLock = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        UpdateStoredTargets();

        TryLockTargetNearCenter();

        hasLock = lockedTarget != null;
    }

    void UpdateStoredTargets()
    {
        // failure to read data, so data is null.
        if (string.IsNullOrEmpty(radar.ReadData())) detectedEntities = new Collider[0];

        detectedEntities = JsonUtility.FromJson<RadarData>(radar.ReadData()).colliders;
    }

    void TryLockTargetNearCenter()
    {
        // * chache if using out of order since the list can update whilst iterating.
        float closestAngle = float.MaxValue;
        Collider closestCol = null;

        foreach (Collider collider in detectedEntities)
        {
            if (collider == null) continue;

            if (closestCol == null)
            {
                closestCol = collider;
                continue;
            }

            Vector3 directionOfTarget = collider.transform.position - headOfPilot.position;
            float angleDeviation = Vector3.Angle(directionOfTarget, headOfPilot.forward);

            if (Vector3.Angle(directionOfTarget, headOfPilot.forward) < closestAngle)
            {
                closestCol = collider;
                closestAngle = angleDeviation;
            }
        }

        if (closestCol != null)
        {
            LockOntoTarget(closestCol);
        }
    }

    void LockOntoTarget(Collider col)
    {
        lockedTarget = col;
    }

    void ReleaseLock()
    {
        lockedTarget = null;
    }

    public string ReadData()
    {
        return JsonUtility.ToJson(new TrackingData()
        {
            lockedTarget = lockedTarget,
            storedDetectedTargets = detectedEntities,
            hasLock = hasLock,
        });
    }
}
