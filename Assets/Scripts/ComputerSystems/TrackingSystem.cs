using System;
using UnityEngine;

[Serializable]
public class TrackingData
{
    public Collider lockedTarget;
    public Collider lockableTarget;
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

    private ComponentBase componentBase;

    private void Awake()
    {
        componentBase = GetComponent<ComponentBase>();
    }

    void FixedUpdate()
    {
        UpdateStoredTargets();

        hasLock = lockedTarget;
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

        if (radar)
        {
            if (string.IsNullOrEmpty(radar.ReadData()))
                detectedRadarEntities = Array.Empty<Collider>();
            else
                detectedRadarEntities = JsonUtility.FromJson<RadarData>(radar.ReadData()).colliders;
        }
        else
        {
            detectedRadarEntities = Array.Empty<Collider>();
        }

        if (generalSensor) // optional module
        {
            if (string.IsNullOrEmpty(generalSensor.ReadData()))
                detectedSensorEntities = Array.Empty<Collider>();
            else
                detectedSensorEntities = JsonUtility.FromJson<GeneralSensorData>(generalSensor.ReadData()).colliders;
        }
        else
        {
            detectedSensorEntities = Array.Empty<Collider>();
        }
    }

    public void TryLockTargetNearCenter(bool ignoreLockedTarget = true)
    {
        // // * chache if using out of order since the list can update whilst iterating.
        // float closestAngle = float.MaxValue;
        // Collider closestCol = null;

        // if (detectedRadarEntities == null) return;
        // if (detectedRadarEntities.Length <= 0) return;

        // foreach (Collider collider in detectedRadarEntities)
        // {
        //     if (!collider) continue;
        //     if (!collider.gameObject.CompareTag(Constants.SHIP_TAG)) continue;

        //     if (!closestCol)
        //     {
        //         closestCol = collider;
        //         continue;
        //     }

        //     Vector3 directionOfTarget = collider.transform.position - headOfPilot.position;
        //     float angleDeviation = Vector3.Angle(directionOfTarget, headOfPilot.forward);

        //     if (Vector3.Angle(directionOfTarget, headOfPilot.forward) < closestAngle)
        //     {
        //         closestCol = collider;
        //         closestAngle = angleDeviation;
        //     }
        // }

        Collider closestCol = GetNearestNearCamCenter(ignoreLockedTarget);

        if (closestCol)
        {
            LockOntoTarget(closestCol);
        }
        else
        {
            ReleaseLock();
        }
    }

    public Collider GetNearestNearCamCenter(bool ignoreLockedTarget = true)
    {
        // * chache if using out of order since the list can update whilst iterating.
        float closestAngle = float.MaxValue;
        Collider closestCol = null;

        if (detectedRadarEntities == null) return null;
        if (detectedRadarEntities.Length <= 0) return null;

        foreach (Collider collider in detectedRadarEntities)
        {
            if (!collider) continue;
            if (!collider.gameObject.CompareTag(Constants.SHIP_TAG)) continue;
            if (collider == lockedTarget && ignoreLockedTarget) continue;

            if (!closestCol)
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

        return closestCol;
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
        if (componentBase.IsComponentDestroyed()) return string.Empty;

        TrackingData data = new TrackingData()
        {
            lockedTarget = lockedTarget,
            storedDetectedRadarTargets = detectedRadarEntities,
            storedDetectedGeneralTargets = detectedSensorEntities,
            hasLock = hasLock,
            lockableTarget = GetNearestNearCamCenter(),
        };

        return JsonUtility.ToJson(data);
    }
}
