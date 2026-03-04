using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RadarData
{
    public Collider[] colliders;
}

public class Radar : ComponentBase, IDataReadable
{
    [SerializeField, Header("Radar")]
    float range = 200f;

    [SerializeField, Range(1, 360)]
    float maxAngle = 90;

    [SerializeField]
    LayerMask radarLayerMask = 1 << 6;

    Collider[] results;

    [SerializeField]
    Collider selfRadarHitBox;

    override protected void Awake()
    {
        base.Awake();
    }

    public string ReadData()
    {
        if (destroyed) return string.Empty; // cant read any data if the radar is destroyed.

        RadarData radarData = new()
        {
            colliders = results,
        };

        return JsonUtility.ToJson(radarData);
    }

    void Sweep()
    {
        List<Collider> foundEntities = Physics.OverlapSphere(transform.position, range, radarLayerMask, QueryTriggerInteraction.Ignore).ToList();

        if (foundEntities == null) results = new Collider[0];

        if (foundEntities.Contains<Collider>(selfRadarHitBox))
        {
            foundEntities.Remove(selfRadarHitBox);
        }

        float halfAngle = maxAngle / 2f;
        Vector3 pos = transform.position;

        if (foundEntities.Count <= 0) results = new Collider[0];
        int count = foundEntities.Count;
        int currentIndex = 0;

        for (int i = 0; i < count; i++)
        {
            Vector3 direction = foundEntities[currentIndex].transform.position - pos;

            if (Vector3.Angle(transform.forward, direction) > halfAngle)
            {
                foundEntities.RemoveAt(currentIndex);
                continue;
            }

            currentIndex++;
        }

        results = foundEntities.ToArray();
    }

    void FixedUpdate()
    {
        Sweep();
    }
}
