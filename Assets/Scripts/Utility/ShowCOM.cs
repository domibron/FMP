using UnityEngine;

public class ShowCOM : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    public Vector3 comWorld;
    public Vector3 comLocal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (rb == null) return;
        Gizmos.DrawSphere(rb.worldCenterOfMass, 0.5f);
        comWorld = rb.worldCenterOfMass;
        comLocal = rb.centerOfMass;
    }
}
