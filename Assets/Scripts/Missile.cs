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
    Collider targetCol = null;
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

    [SerializeField]
    AudioSource audioSource;

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
                    targetCol = data.lockedTarget;
                    target = data.lockedTarget.transform.position;
                }
                else
                {
                    trackingSystem.TryLockTargetNearCenter(allowUnlock: false);
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
                if (data.validTarget)
                {
                    target = data.target;
                    targetCol = data.targetCol;
                }
            }

            if (armed)
            {
                trackingSystem.TryLockTargetNearCenter(allowUnlock: false);

                if (!string.IsNullOrEmpty(trackingSystem.ReadData()))
                {
                    TrackingData tData = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

                    if (tData.hasLock && tData.lockedTarget && tData.lockedTarget == targetCol)// && Vector3.Distance(target.Value, tData.lockedTarget.transform.position) < 1)
                    {
                        target = tData.lockedTarget.transform.position;
                        targetCol = tData.lockedTarget;
                        antenna = null; // disconnect from antenna.
                        print("Found target, disconnecting from ship");
                    }
                }
            }
        }

        if (!isActive && !DebugActivate) return;

        if (!audioSource.isPlaying) audioSource.Play();

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

            Vector3 targetPos = (target.Value + displacementSpeed) - ((rb.linearVelocity + (thruster.GetForceDirection() / rb.mass)) * Time.deltaTime);
            Vector3 targDir = targetPos - rb.transform.position;

            // neededVel = (neededVel / Time.deltaTime) - rb.linearVelocity;
            Vector3 cross = Vector3.Cross(targDir, rb.linearVelocity);
            float dot = Vector3.Dot(targDir, rb.linearVelocity);

            Vector3 neededLook = Quaternion.AngleAxis(-Mathf.Acos(dot) * Mathf.Rad2Deg, cross) * targDir;

            Vector3 localVel = rb.transform.InverseTransformDirection(rb.linearVelocity);

            localVel.z = 0;

            localVel = rb.transform.TransformDirection(localVel);

            // rb.transform.LookAt(rb.transform.position + neededLook);
            rb.transform.LookAt(targetPos - localVel);

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
