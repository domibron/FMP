using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class RadarData
{
    public Collider[] collider;
}

public class Radar : ComponentBase, IDataReadable
{
    [SerializeField]
    float range = 200f;

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
            collider = results,
        };

        return JsonUtility.ToJson(radarData);
    }

    void Sweep()
    {
        results = Physics.OverlapSphere(transform.position, range, radarLayerMask, QueryTriggerInteraction.Ignore);

        if (results.Contains<Collider>(selfRadarHitBox))
        {
            Collider[] _ = new Collider[results.Length - 1];

            int index = 0;

            foreach (var col in results)
            {
                if (col == selfRadarHitBox) continue;

                _[index] = col;
                index++;
            }

            results = _;
        }
    }

    void FixedUpdate()
    {
        Sweep();
    }
}
