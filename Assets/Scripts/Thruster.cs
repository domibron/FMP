using UnityEngine;

public class Thruster : ComponentBase
{
    [SerializeField]
    private Rigidbody shipRb;

    [SerializeField]
    private Vector3 forceDirection;

    [SerializeField]
    float consumptionPerForce = 0.01f;

    [SerializeField]
    FuelTank fuelTank;

    private float targetForce = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {

        TickThruster();
    }

    public void SetThrusterForce(float force)
    {
        targetForce = Mathf.Max(force, 0f);// prevents negative forces.
    }

    public void AddToThrusterForce(float forceToAdd)
    {
        targetForce += Mathf.Max(forceToAdd, 0f);
    }

    public float GetThrusterForce()
    {
        return targetForce;
    }

    private void TickThruster()
    {
        if (fuelTank != null)
        {
            if (fuelTank.Consume(targetForce * Time.deltaTime * consumptionPerForce))
            {
                shipRb.AddForceAtPosition(transform.TransformDirection(-forceDirection.normalized) * targetForce * Time.deltaTime, transform.position, ForceMode.Impulse);
            }
        }
        else // fuel tank broken but still allow flight.
        {
            shipRb.AddForceAtPosition(transform.TransformDirection(-forceDirection.normalized) * targetForce * Time.deltaTime, transform.position, ForceMode.Impulse);
        }
    }

    public void PulseThruster(float force)
    {
        if (fuelTank != null)
        {
            if (fuelTank.Consume(force * consumptionPerForce))
            {
                shipRb.AddForceAtPosition(transform.TransformDirection(-forceDirection.normalized) * force, transform.position, ForceMode.Impulse);
            }
        }
        else // fuel tank broken but still allow flight.
        {
            shipRb.AddForceAtPosition(transform.TransformDirection(-forceDirection.normalized) * force, transform.position, ForceMode.Impulse);
        }
    }

    public Vector3 GetForceDirection()
    {
        // Debug.DrawLine(transform.position, transform.position + Quaternion.Inverse(transform.localRotation) * -forceDirection.normalized, Color.red);
        return Quaternion.Inverse(transform.localRotation) * -forceDirection.normalized;
        // return transform.TransformDirection(-forceDirection.normalized);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.TransformDirection(forceDirection.normalized) * 3f));
    }
}
