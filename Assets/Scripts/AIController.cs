using System;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    InputIntermediate inputIntermediate;

    [SerializeField]
    float maxMoveSpeed = 10f;

    [SerializeField]
    float maxRotationSpeed = 10f;

    public Transform[] flightPath;

    private int currentFlightPathNode = 0;

    private float missileCoolDown = 10f;

    private float currentMissileCoolDown = 0f;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    TrackingSystem trackingSystem;

    [SerializeField]
    WeaponManager weaponManager;

    [SerializeField]
    RadarPing radarPing;

    float missileLock = 0;

    private float cmCoolDown = 10f;

    private float currentCMCoolDown = 0;

    [SerializeField]
    ComponentBase[] allComps;

    [SerializeField]
    ComponentBase computer;

    void Start()
    {
        flightPath = DrawFlightPath.Instance.GetFlightPath();

        radarPing.OnPinged += OnBeingLocked;

        StartCoroutine(SelfDet());
        StartCoroutine(FlightNav());
        StartCoroutine(EngageEnemy());
        StartCoroutine(Countermeasures());
    }

    private void OnBeingLocked(Collider collider)
    {
        if (collider.gameObject.CompareTag(Constants.MISSILE_TAG))
        {
            missileLock = 0.3f;
        }
    }

    void FixedUpdate()
    {
        if (currentMissileCoolDown > 0) currentMissileCoolDown -= Time.fixedDeltaTime;
        if (currentCMCoolDown > 0) currentCMCoolDown -= Time.fixedDeltaTime;
        if (missileLock > 0) missileLock -= Time.fixedDeltaTime;
    }

    IEnumerator SelfDet()
    {
        while (true)
        {
            if (computer.IsComponentDestroyed())
            {
                GameManager.Instance.SelfDestruct(Team.TeamType.TeamB);
            }
            else
            {
                float collectiveAvg = 0;

                foreach (var comp in allComps)
                {
                    collectiveAvg += comp.HealthNormalized;
                }

                collectiveAvg /= allComps.Length;

                if (collectiveAvg < 0.8f)
                {
                    GameManager.Instance.SelfDestruct(Team.TeamType.TeamB);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Countermeasures()
    {
        while (true)
        {
            if (inputIntermediate.GetCounterMPressed())
            {
                inputIntermediate.SetCounterMPressed(false);
                yield return new WaitForFixedUpdate();
            }

            if (missileLock > 0 && currentCMCoolDown <= 0)
            {
                currentCMCoolDown = cmCoolDown;
                inputIntermediate.SetCounterMPressed(true);

            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator EngageEnemy()
    {
        while (true)
        {
            while (JsonUtility.FromJson<WeaponManagerData>(weaponManager.ReadData()).GetSelectedWeapon != WeaponType.Missile)
            {
                inputIntermediate.SetSwitchPressed(true);
                yield return new WaitForFixedUpdate();

                inputIntermediate.SetSwitchPressed(false);
                yield return new WaitForFixedUpdate();
            }

            if (inputIntermediate.GetFirePressed())
            {
                inputIntermediate.SetFirePressed(false);
                yield return new WaitForFixedUpdate();
            }

            if (!string.IsNullOrEmpty(trackingSystem.ReadData()))
            {
                TrackingData data = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

                if (!data.hasLock && data.storedDetectedRadarTargets.Count() > 0)
                {
                    trackingSystem.TryLockTargetNearCenter();
                }
                else if (currentMissileCoolDown <= 0)
                {
                    currentMissileCoolDown = missileCoolDown;
                    inputIntermediate.SetFirePressed(true);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FlightNav()
    {
        while (true)
        {
            // while (Mathf.Acos(Vector3.Dot(rb.transform.forward, (flightPath[currentFlightPathNode].position - rb.transform.position).normalized)) * Mathf.Rad2Deg > 10f)
            // {
            //     // print(math.acos(Vector3.Dot(rb.transform.forward, flightPath[currentFlightPathNode].position - rb.transform.position)));

            //     // Quaternion rotNeeded = Quaternion.FromToRotation(rb.transform.forward, (flightPath[currentFlightPathNode].position - rb.transform.position).normalized);

            //     // Vector3 eulerNeeded = rb.transform.InverseTransformDirection(rotNeeded.eulerAngles);

            //     // Vector3 angVel = rb.transform.InverseTransformDirection(rb.angularVelocity);

            //     // Vector3 input = Vector3.zero;

            //     // if (eulerNeeded.x > 0 && angVel.x < maxRotationSpeed)
            //     // {
            //     //     input.x = 1;
            //     // }
            //     // else if (eulerNeeded.x < 0 && angVel.x > -maxRotationSpeed)
            //     // {
            //     //     input.x = -1;
            //     // }

            //     // if (eulerNeeded.y > 0 && angVel.y < maxRotationSpeed)
            //     // {
            //     //     input.y = 1;
            //     // }
            //     // else if (eulerNeeded.y < 0 && angVel.y > -maxRotationSpeed)
            //     // {
            //     //     input.y = -1;
            //     // }

            //     // // if (eulerNeeded.z > 0 && angVel.z < maxRotationSpeed)
            //     // // {
            //     // //     input.z = 1;
            //     // // }
            //     // // else if (eulerNeeded.z < 0 && angVel.z > -maxRotationSpeed)
            //     // // {
            //     // //     input.z = -1;
            //     // // }

            //     // float GetInputFromRes(float needed, float current, float max)
            //     // {
            //     //     if (needed > 0 && current < max)
            //     //     {
            //     //         return 1;
            //     //     }
            //     //     else if (needed < 0 && current > -max)
            //     //     {
            //     //         return -1;
            //     //     }
            //     //     else
            //     //     {
            //     //         return 0;
            //     //     }
            //     // }
            //     // print(eulerNeeded);
            //     // inputIntermediate.SetLookVector(input);


            //     yield return new WaitForFixedUpdate();
            // }

            while (Vector3.Distance(rb.transform.position, flightPath[currentFlightPathNode].position) > 5f)
            {
                // TODO: Fix this later.
                rb.transform.LookAt(flightPath[currentFlightPathNode].position);


                Vector3 neededMov = flightPath[currentFlightPathNode].position - rb.transform.position;

                neededMov = rb.transform.InverseTransformDirection(neededMov);

                Vector3 rbLinVel = rb.transform.InverseTransformDirection(rb.linearVelocity);

                Vector3 input = Vector3.zero;

                if (neededMov.x > 0 && rbLinVel.x < maxMoveSpeed)
                {
                    input.x = 1;
                }
                else if (neededMov.x < 0 && rbLinVel.x > -maxMoveSpeed)
                {
                    input.x = -1;
                }

                if (neededMov.y > 0 && rbLinVel.y < maxMoveSpeed)
                {
                    input.y = 1;
                }
                else if (neededMov.y < 0 && rbLinVel.y > -maxMoveSpeed)
                {
                    input.y = -1;
                }

                if (neededMov.z > 0 && rbLinVel.z < maxMoveSpeed)
                {
                    input.z = 1;
                }
                else if (neededMov.z < 0 && rbLinVel.z > -maxMoveSpeed)
                {
                    input.z = -1;
                }

                inputIntermediate.SetMoveVector(input);



                yield return new WaitForFixedUpdate();
            }

            currentFlightPathNode++;

            if (currentFlightPathNode >= flightPath.Length) currentFlightPathNode = 0;
        }
    }

}
