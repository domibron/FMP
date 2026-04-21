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

    [SerializeField]
    LayerMask validBlockerLayers = (1 << 0) | (1 << 9);

    [SerializeField]
    LayerMask countermeasureLayer = (1 << 9);

    [SerializeField]
    bool startActive = true;

    bool isActive = false;

    private BoxCollider bCollider;
    private SphereCollider sphCollider;


    override protected void Awake()
    {
        isActive = startActive;

        base.Awake();

        bCollider = GetComponent<BoxCollider>();

        if (!bCollider)
        {
            sphCollider = GetComponent<SphereCollider>();
        }
    }

    void Update()
    {

        selfRadarHitBox.gameObject.SetActive(isActive);

    }

    public string ReadData()
    {
        if (destroyed || !isActive) return string.Empty; // cant read any data if the radar is destroyed.

        RadarData radarData = new()
        {
            colliders = results,
        };

        return JsonUtility.ToJson(radarData);
    }

    void Sweep()
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

        if (foundEntities == null) results = Array.Empty<Collider>();

        if (foundEntities.Contains<Collider>(selfRadarHitBox))
        {
            foundEntities.Remove(selfRadarHitBox);
        }

        float halfAngle = maxAngle / 2f;
        Vector3 pos = transform.position;

        if (foundEntities.Count <= 0) results = Array.Empty<Collider>();
        int count = foundEntities.Count;
        int currentIndex = 0;

        for (int i = 0; i < count; i++)
        {
            if (currentIndex >= foundEntities.Count) break; // exceeded list bounds, exit to prevent errors.

            Vector3 entityPos = foundEntities[currentIndex].transform.position;


            Vector3 direction = entityPos - pos;

            if (Vector3.Angle(transform.forward, direction) > halfAngle)
            {
                foundEntities.RemoveAt(currentIndex);
                continue;
            }

            RaycastHit[] hits = Physics.RaycastAll(transform.position, (entityPos - transform.position).normalized, Vector3.Distance(entityPos, transform.position), validBlockerLayers);

            bool isBlocked = false;

            foreach (var hit in hits)
            {
                if (currentIndex >= foundEntities.Count) break;
                if (!hit.collider) continue;
                if (hit.collider == foundEntities[currentIndex] || hit.collider == selfRadarHitBox) continue;
                if (hit.collider.tag == Constants.SHIP_TAG) continue;

                // print(hit.collider.transform.name);

                // foundEntities.RemoveAt(currentIndex);
                isBlocked = true;
                // continue;
            }

            if (isBlocked)
            {
                foundEntities.RemoveAt(currentIndex);
                continue;
            }

            currentIndex++;
        }

        if (foundEntities.Count <= 0) results = Array.Empty<Collider>();

        results = foundEntities.ToArray();
    }

    void FixedUpdate()
    {
        Sweep();
    }

    public void Activate()
    {
        isActive = true;
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
