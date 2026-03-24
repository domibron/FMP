using System;
using UnityEngine;

public class Missile : ComponentBase, IActivateable
{
    [SerializeField, Header("Missile")]
    Thruster thruster;

    Antenna antenna;

    bool isActive = false;

    Rigidbody rb;

    float ejectionForce = 15f;

    [SerializeField]
    float missileSpeed = 360f;

    [SerializeField]
    bool DebugActivate = false;

    Vector3? target = null;

    bool armed = false;

    float minDistanceToArm = 5f;

    float currentAliveTime = 0;

    [SerializeField]
    TrackingSystem trackingSystem;

    [SerializeField]
    Warhead warhead;

    const float MAX_ALIVE_TIME = 60f; // seconds
    const float MIN_DETONATION_DIST = 3f; // seconds

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();

        warhead.OnDetonation += OnDetonation;
    }

    private void OnDetonation()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        // acquire target.
        if (!antenna)
        {
            // Try and use self acquire target.

            if (string.IsNullOrEmpty(trackingSystem.ReadData()))
            {
                // target = null;
            }
            else
            {
                TrackingData data = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

                if (data.hasLock)
                {
                    target = data.lockedTarget.transform.position;
                }
                else
                {
                    // target = null;
                }
            }
        }
        else
        {

            if (!antenna || string.IsNullOrEmpty(antenna.GetData(rb.transform.position)))
            {
                // target = null;
            }
            else
            {
                // Should have antenna to antenna interaction but this roughly simulates that anyway.
                AntennaData data = JsonUtility.FromJson<AntennaData>(antenna.GetData(rb.transform.position));
                // print(antenna.GetData(rb.transform.position));
                // target = data.validTarget ? data.target : null;
                if (data.validTarget) target = data.target;
            }
        }

        if (!isActive && !DebugActivate) return;

        thruster.SetThrusterForce(missileSpeed);

        currentAliveTime += Time.deltaTime;

        if (currentAliveTime >= MAX_ALIVE_TIME)
        {
            // EXPLODE!
            Explode();
            return;
        }

        // can we arm?
        if (!armed)
        {
            if (Vector3.Distance(antenna.transform.position, rb.transform.position) > minDistanceToArm)
            {
                armed = true;
                warhead.Arm();
            }
            else
            {
                return; // Cannot do anything.
            }
        }

        if (target != null)
        {
            rb.transform.LookAt(target.Value - rb.linearVelocity);

            // rb.transform.LookAt(transform.position + (target.Value - (transform.position + rb.linearVelocity)));

            if (Vector3.Distance(target.Value, rb.transform.position) <= MIN_DETONATION_DIST)
            {
                // EXPLODE!
                Explode();
            }
        }

    }

    private void Explode()
    {
        warhead.Detonate();
    }

    public void SetUpMissile(Antenna antenna)
    {
        // link the missile.
        this.antenna = antenna;
        isActive = false;
    }

    public void Activate()
    {
        if (isActive) return;
        isActive = true;
        rb.isKinematic = false; // This first before doing any velocity stuff as kinematic will remove all our velocity stuff if disabled after it.

        if (transform.parent != null && transform.parent.GetComponentInParent<Rigidbody>() != null)
        {
            rb.linearVelocity = transform.parent.GetComponentInParent<Rigidbody>().linearVelocity + ((-transform.up) * ejectionForce) + rb.transform.forward * missileSpeed;
            // print("vel " + (transform.parent.GetComponentInParent<Rigidbody>().linearVelocity + ((-transform.up) * 15f)));
        }

        thruster.SetThrusterForce(missileSpeed);
        transform.parent = null;
        // activate thruster.
    }


}
