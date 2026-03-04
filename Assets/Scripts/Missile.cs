using UnityEngine;

public class Missile : ComponentBase, IActivateable
{
    [SerializeField, Header("Missile")]
    Thruster thruster;

    Antenna antenna;

    bool isActive = false;

    Rigidbody rb;

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
        transform.parent = null;
        rb.isKinematic = false;
        // activate thruster.
    }
}
