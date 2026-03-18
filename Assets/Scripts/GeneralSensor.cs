using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GeneralSensorData
{
    public Collider[] colliders;
}

public class GeneralSensor : ComponentBase, IDataReadable
{
    [SerializeField]
    float range = 50f;

    [SerializeField]
    LayerMask radarLayerMask = 1 << 6;

    Collider[] results;

    [SerializeField]
    Collider selfRadarHitBox;

    public string ReadData()
    {
        if (destroyed) return string.Empty; // cant read any data if the radar is destroyed.

        RadarData radarData = new()
        {
            colliders = results,
        };

        return JsonUtility.ToJson(radarData);
    }

    void FixedUpdate()
    {
        Scan();
    }

    void Scan()
    {
        List<Collider> foundEntities = Physics.OverlapSphere(transform.position, range, radarLayerMask, QueryTriggerInteraction.Ignore).ToList();

        if (foundEntities == null) results = new Collider[0];

        if (foundEntities.Contains<Collider>(selfRadarHitBox))
        {
            foundEntities.Remove(selfRadarHitBox);
        }

        if (foundEntities.Count <= 0) results = new Collider[0];

        results = foundEntities.ToArray();
    }
}
