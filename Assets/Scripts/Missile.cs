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
    Vector3? targetLast = null;

    bool armed = false;

    [SerializeField]
    float minDistanceToArm = 5f;

    float currentAliveTime = 0;

    [SerializeField]
    TrackingSystem trackingSystem;

    [SerializeField]
    Radar radar;

    [SerializeField]
    Warhead warhead;

    [SerializeField]
    float maxAliveTime = 60f; // seconds

    [SerializeField]
    float detonationDistance = 5f;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();

        warhead.OnDetonation += OnDetonation;
    }

    protected override void DestroyComponent()
    {
        base.DestroyComponent();


        if (!warhead.Detonate())
        {
            Destroy(gameObject);
        }
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
                    trackingSystem.TryLockTargetNearCenter();
                    // target = null;
                }
            }
        }
        else
        {
            if (!antenna || !rb.transform)
            {
                // target = null;
            }
            else if (string.IsNullOrEmpty(antenna.GetData(rb.transform.position)))
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

            if (armed)
            {
                trackingSystem.TryLockTargetNearCenter();

                if (!string.IsNullOrEmpty(trackingSystem.ReadData()))
                {
                    TrackingData tData = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

                    if (tData.hasLock && tData.lockedTarget)
                    {
                        target = tData.lockedTarget.transform.position;
                        antenna = null; // disconnect from antenna.
                        print("Found target, disconnecting from ship");
                    }
                }
            }
        }

        if (!isActive && !DebugActivate) return;


        thruster.SetThrusterForce(missileSpeed);

        currentAliveTime += Time.deltaTime;

        if (currentAliveTime >= maxAliveTime)
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
            if (!targetLast.HasValue)
            {
                targetLast = target; // wait to sample
            }

            Vector3 displacementSpeed = (target.Value - targetLast.Value) / Time.deltaTime;

            // Vector3 neededVel = (target.Value + displacementSpeed) - (transform.position + (rb.linearVelocity * Time.deltaTime));

            // neededVel = (neededVel / Time.deltaTime) - rb.linearVelocity;


            rb.transform.LookAt((target.Value + displacementSpeed) - (rb.linearVelocity * Time.deltaTime));

            // rb.transform.LookAt(transform.position + (target.Value - (transform.position + rb.linearVelocity)));

            targetLast = target;

            if (Vector3.Distance(target.Value, rb.transform.position) <= detonationDistance)
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

        radar.Activate();

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

    void OnCollisionEnter(Collision collision)
    {
        if (isActive && armed)
        {
            Explode();
        }
    }


}
