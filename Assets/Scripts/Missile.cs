using UnityEngine;

public class Missile : ComponentBase, IActivateable
{
    [SerializeField, Header("Missile")]
    Thruster thruster;

    Antenna antenna;

    bool isActive = false;

    Rigidbody rb;

    float ejectionForce = 15f;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isActive)
            thruster.SetThrusterForce(360f);
    }

    public void SetUpMissile(Antenna antenna)
    {
        this.antenna = antenna;
        isActive = false;
    }

    public void Activate()
    {
        isActive = true;
        rb.isKinematic = false;

        if (transform.parent != null && transform.parent.GetComponentInParent<Rigidbody>() != null)
        {
            rb.linearVelocity = transform.parent.GetComponentInParent<Rigidbody>().linearVelocity + ((-transform.up) * ejectionForce);
            // print("vel " + (transform.parent.GetComponentInParent<Rigidbody>().linearVelocity + ((-transform.up) * 15f)));
        }

        thruster.SetThrusterForce(360f);
        transform.parent = null;
        // activate thruster.
    }
}
