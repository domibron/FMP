using UnityEngine;

public class HangarTrigger : MonoBehaviour
{
    [SerializeField]
    Hangar hangar;

    void OnTriggerEnter(Collider other)
    {
        hangar.OnShipEnter(other.transform.root.gameObject);
    }
}
