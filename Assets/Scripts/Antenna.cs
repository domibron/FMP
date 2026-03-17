using System;
using UnityEngine;

[Serializable]
public class AntennaData
{
    public Vector3 target;
    public bool validTarget;
}

public class Antenna : ComponentBase
{
    [SerializeField]
    float maxRange = 500f;

    [SerializeField]
    TrackingSystem trackingSystem;


    public string GetData(Vector3 callerWorldPosition)
    {
        if (destroyed)
        {
            // print("destroyed");
            return string.Empty;
        }

        if (Vector3.Distance(callerWorldPosition, transform.position) > maxRange)
        {
            // print("out of range");
            return string.Empty;
        }

        if (string.IsNullOrEmpty(trackingSystem.ReadData()))
        {
            // print("cannot get data");
            return string.Empty;
        }



        TrackingData data = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

        if (!data.hasLock)
        {
            // print("No lock");
            return JsonUtility.ToJson(new AntennaData()
            {
                target = Vector3.zero,
                validTarget = false,
            });
        }

        return JsonUtility.ToJson(new AntennaData()
        {
            target = data.lockedTarget.transform.position,
            validTarget = true,
        });
    }
}
