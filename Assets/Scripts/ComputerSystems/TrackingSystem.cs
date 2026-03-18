using System;
using UnityEngine;

[Serializable]
public class TrackingData
{
    public Collider lockedTarget;
    public Collider[] storedDetectedRadarTargets;
    public Collider[] storedDetectedGeneralTargets;
    public bool hasLock;
}

public class TrackingSystem : MonoBehaviour, IDataReadable
{
    [SerializeField]
    Radar radar;

    [SerializeField]
    Transform headOfPilot;

    [SerializeField]
    GeneralSensor generalSensor;

    Collider[] detectedRadarEntities;
    Collider[] detectedSensorEntities;

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

        // TryLockTargetNearCenter();

        hasLock = lockedTarget != null;
    }

    // void OnGUI()
    // {
    //     GUILayout.Label($"<size={Screen.height / 20f}>Locked: {hasLock}");
    //     if (hasLock)
    //     {
    //         GUILayout.Label($"<size={Screen.height / 20f}>Target Name: {lockedTarget.transform.parent?.name}");
    //         GUILayout.Label($"<size={Screen.height / 20f}>Target Pos: {lockedTarget.transform.position}");
    //         GUILayout.Label($"<size={Screen.height / 20f}>Target Type: {lockedTarget.tag}");
    //     }
    // }

    void UpdateStoredTargets()
    {
        // failure to read data, so data is null.

        if (string.IsNullOrEmpty(radar.ReadData()))
            detectedRadarEntities = Array.Empty<Collider>();
        else
            detectedRadarEntities = JsonUtility.FromJson<RadarData>(radar.ReadData()).colliders;

        if (string.IsNullOrEmpty(generalSensor.ReadData()))
            detectedSensorEntities = Array.Empty<Collider>();
        else
            detectedSensorEntities = JsonUtility.FromJson<GeneralSensorData>(generalSensor.ReadData()).colliders;
    }

    public void TryLockTargetNearCenter()
    {
        // * chache if using out of order since the list can update whilst iterating.
        float closestAngle = float.MaxValue;
        Collider closestCol = null;

        if (detectedRadarEntities == null) return;
        if (detectedRadarEntities.Length <= 0) return;

        foreach (Collider collider in detectedRadarEntities)
        {
            if (collider == null) continue;
            if (collider.tag != Constants.SHIP_TAG) continue;

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
        TrackingData data = new TrackingData()
        {
            lockedTarget = lockedTarget,
            storedDetectedRadarTargets = detectedRadarEntities,
            storedDetectedGeneralTargets = detectedSensorEntities,
            hasLock = hasLock,
        };

        return JsonUtility.ToJson(data);
    }
}
