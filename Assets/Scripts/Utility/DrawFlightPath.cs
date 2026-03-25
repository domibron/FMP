using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawFlightPath : MonoBehaviour
{
    public static DrawFlightPath Instance;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Debug.LogWarning("Two flightpaths detected");
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public Transform[] GetFlightPath()
    {
        List<Transform> children = transform.GetComponentsInChildren<Transform>().ToList();

        bool hasSelf = children[0] == transform;

        if (hasSelf)
        {
            children.RemoveAt(0);
        }

        return children.ToArray();
    }

    void OnDrawGizmos()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        bool hasSelf = children[0] == transform;

        if (hasSelf && children.Length <= 1) return;
        else if (!hasSelf && children.Length <= 0) return;

        for (int i = 0; i < children.Length; i++)
        {
            if (hasSelf && i == 0) continue;

            Gizmos.DrawSphere(children[i].position, 1f);

            int prev = i - 1;

            if (hasSelf && prev <= 0)
                prev = children.Length - 1;
            else if (!hasSelf && prev <= -1)
                prev = children.Length - 1;

            Gizmos.DrawLine(children[prev].position, children[i].position);
        }
    }
}
