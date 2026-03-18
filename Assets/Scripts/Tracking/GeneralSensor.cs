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

    [SerializeField]
    LayerMask validBlockerLayers = (1 << 0) | (1 << 9);

    [SerializeField]
    LayerMask countermeasureLayer = (1 << 9);
    
    private BoxCollider bCollider;
    private SphereCollider sphCollider;
    
    protected override void Awake()
    {
        base.Awake();
        
        bCollider = GetComponent<BoxCollider>();
    }

    public string ReadData()
    {
        if (destroyed) return string.Empty; // cant read any data if the radar is destroyed.

        GeneralSensorData generalSensorData = new()
        {
            colliders = results,
        };

        return JsonUtility.ToJson(generalSensorData);
    }

    void FixedUpdate()
    {
        Scan();
    }

    void Scan()
    {
        if (bCollider)
        {
            if (Physics.CheckBox(transform.position + bCollider.center, bCollider.size / 2f, transform.rotation,
                    countermeasureLayer, QueryTriggerInteraction.Ignore))
            {
                results = Array.Empty<Collider>();
                return;
            }
        }
        else if (sphCollider)
        {
            if (Physics.CheckSphere(transform.position + sphCollider.center, sphCollider.radius, countermeasureLayer, QueryTriggerInteraction.Ignore))
            {
                results = Array.Empty<Collider>();
                return;
            }
        }

        List<Collider> foundEntities = Physics.OverlapSphere(transform.position, range, radarLayerMask, QueryTriggerInteraction.Ignore).ToList();

        if (foundEntities.Count <= 0) results = Array.Empty<Collider>();

        if (foundEntities.Contains<Collider>(selfRadarHitBox))
        {
            foundEntities.Remove(selfRadarHitBox);
        }

        int count = foundEntities.Count;
        int currentIndex = 0;

        for (int i = 0; i < count; i++)
        {
            if (currentIndex >= foundEntities.Count) break; // exceeded list bounds, exit to prevent errors.

            Vector3 entityPos = foundEntities[currentIndex].transform.position;



            RaycastHit[] hits = Physics.RaycastAll(transform.position, (entityPos - transform.position).normalized, Vector3.Distance(entityPos, transform.position), validBlockerLayers);

            foreach (var hit in hits)
            {
                if (currentIndex >= foundEntities.Count) break;
                if (!hit.collider) continue;
                if (hit.collider == foundEntities[currentIndex] || hit.collider == selfRadarHitBox) continue; // should be impossible to get hit
                if (hit.collider.CompareTag(Constants.SHIP_TAG)) continue;

                foundEntities.RemoveAt(currentIndex);
                continue;
            }

            currentIndex++;
        }

        if (foundEntities.Count <= 0) results = Array.Empty<Collider>();

        results = foundEntities.ToArray();
    }
}
