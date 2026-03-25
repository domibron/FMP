using UnityEngine;

public class Thruster : ComponentBase
{
    [SerializeField, Header("Thruster")]
    private Rigidbody rb;

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
        if (destroyed) return;

        if (fuelTank != null)
        {
            if (fuelTank.Consume(targetForce * Time.deltaTime * consumptionPerForce))
            {
                rb.AddForceAtPosition(transform.TransformDirection(GetForceDirectionVector()) * targetForce * Time.deltaTime, transform.position, ForceMode.Impulse);
            }
        }
        else // fuel tank broken but still allow flight.
        {
            rb.AddForceAtPosition(transform.TransformDirection(GetForceDirectionVector()) * targetForce * Time.deltaTime, transform.position, ForceMode.Impulse);
        }
    }

    public void PulseThruster(float force)
    {
        if (destroyed) return;

        if (fuelTank != null)
        {
            if (fuelTank.Consume(force * consumptionPerForce))
            {
                rb.AddForceAtPosition(transform.TransformDirection(GetForceDirectionVector()) * force, transform.position, ForceMode.Impulse);
            }
        }
        else // fuel tank broken but still allow flight.
        {
            rb.AddForceAtPosition(transform.TransformDirection(GetForceDirectionVector()) * force, transform.position, ForceMode.Impulse);
        }
    }

    public Vector3 GetForceDirection()
    {
        // Debug.DrawLine(transform.position, transform.position + Quaternion.Inverse(transform.localRotation) * -forceDirection.normalized, Color.red);
        return Quaternion.Inverse(transform.localRotation) * GetForceDirectionVector();
        // return transform.TransformDirection(-forceDirection.normalized);
    }

    public Vector3 GetForceDirectionVector()
    {
        return -forceDirection.normalized;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.TransformDirection(forceDirection.normalized) * 3f));
    }
}
