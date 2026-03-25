using System;
using UnityEngine;

public class RadarPing : MonoBehaviour
{
    public event Action<Collider> OnPinged;

    public void PingSystem(Collider source)
    {
        OnPinged?.Invoke(source);
    }
}
