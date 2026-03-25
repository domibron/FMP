using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LockableSprites
{
    public Sprite normal;
    public Sprite lockable;
    public Sprite locked;
    public Sprite missile;
}

public class HUD : MonoBehaviour
{
    [SerializeField]
    Camera HudCamera;

    [SerializeField]
    TrackingSystem trackingSystem;

    [SerializeField]
    Transform trackedObjectsMarkers;

    [SerializeField]
    GameObject markerPrefab;

    [SerializeField]
    LockableSprites lockSprites;

    Dictionary<Collider, GameObject> markers = new Dictionary<Collider, GameObject>();

    List<Collider> lockables = new List<Collider>();

    Collider lockedTarget;
    Collider lockableTarget;

    [SerializeField]
    Transform cannonTar;

    Vector3 magicCannonPoint;

    void Start()
    {
        magicCannonPoint = (HudCamera.transform.position + (HudCamera.transform.up * -2f)) + (HudCamera.transform.forward * 100f);
    }

    void OnDrawGizmos()
    {
        if (HudCamera == null) return;
        Gizmos.DrawSphere(magicCannonPoint, 5f);
        Gizmos.DrawLine(HudCamera.transform.position, magicCannonPoint);
    }

    void FixedUpdate()
    {
        Vector3 moveAmount = ((HudCamera.transform.position + (HudCamera.transform.up * -2f)) + (HudCamera.transform.forward * 100f)) - magicCannonPoint;

        moveAmount *= 100f / Cannon.BULLET_SPEED;

        magicCannonPoint += moveAmount;


        cannonTar.position = HudCamera.WorldToScreenPoint(magicCannonPoint);


        UpdateLockables();
        UpdateMarkers();
    }

    void UpdateLockables()
    {
        if (string.IsNullOrEmpty(trackingSystem.ReadData()))
        {
            lockables.Clear();
            return;
        }

        TrackingData trackingData = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

        Collider[] cols = trackingData.storedDetectedRadarTargets;

        lockables.Clear();

        foreach (Collider col in cols)
        {
            lockables.Add(col);
            CreateMarker(col);
        }

        cols = trackingData.storedDetectedGeneralTargets;

        foreach (Collider col in cols)
        {
            if (lockables.Contains(col)) continue;

            lockables.Add(col);
            CreateMarker(col);
        }

        lockedTarget = trackingData.lockedTarget;
        lockableTarget = trackingData.lockableTarget;
    }

    void UpdateMarkers()
    {
        List<Collider> toRemove = new List<Collider>();


        foreach (var marker in markers.Keys)
        {
            if (!marker)
            {
                toRemove.Add(marker);
                continue;
            }

            if (!lockables.Contains(marker))
            {
                toRemove.Add(marker);
            }

            Vector3 pos = marker.transform.position;
            pos = HudCamera.WorldToScreenPoint(pos);

            markers[marker].transform.position = pos;

            if (pos.z < 0)
            {
                markers[marker].SetActive(false);
            }
            else
            {
                markers[marker].SetActive(true);
            }

            Image image = markers[marker].GetComponent<Image>();

            if (marker.gameObject.CompareTag(Constants.SHIP_TAG))
            {
                if (lockedTarget && lockedTarget == marker)
                {
                    image.sprite = lockSprites.locked;
                }
                else if (lockableTarget && lockableTarget == marker)
                {
                    image.sprite = lockSprites.lockable;
                }
                else
                {
                    image.sprite = lockSprites.normal;
                }
            }
            else
            {
                image.sprite = lockSprites.missile;
            }

            // float scale = Mathf.Tan(Mathf.Atan(1f / 10f)) * ( Vector3.Distance(marker.transform.position, HudCamera.transform.position) / 10f);
            // markers[marker].transform.localScale = new Vector3(scale, scale, scale);
        }

        foreach (var marker in toRemove)
        {
            RemoveMarker(marker);
        }
    }

    void RemoveMarker(Collider col)
    {
        Destroy(markers[col]);
        markers.Remove(col);
    }

    void CreateMarker(Collider col)
    {
        if (markers.ContainsKey(col)) return;

        GameObject marker = Instantiate(markerPrefab, trackedObjectsMarkers);

        Vector3 pos = marker.transform.position;
        pos = HudCamera.WorldToScreenPoint(pos);

        marker.transform.position = pos;

        if (pos.z < 0)
        {
            marker.SetActive(false);
        }
        else
        {
            marker.SetActive(true);
        }

        // float scale = Mathf.Atan(Mathf.Atan(1f / 10f)) * (Vector3.Distance(col.transform.position, HudCamera.transform.position));
        // marker.transform.localScale = new Vector3(scale, scale, scale);

        markers.Add(col, marker);
    }

}
