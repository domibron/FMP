using UnityEngine;

public class AntennaData
{
    public Vector3? target;
}

public class Antenna : ComponentBase
{
    [SerializeField]
    float maxRange = 500f;

    [SerializeField]
    TrackingSystem trackingSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetData(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) > maxRange)
        {
            return string.Empty;
        }

        return JsonUtility.ToJson(new AntennaData()
        {
            target = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData())?.lockedTarget?.transform.position,
        });
    }
}
