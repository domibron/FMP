using UnityEngine;

public class FlightController : MonoBehaviour
{
    // forward facing thrusters (next to cockpit)
    // Could replace with getting thruster direction instead then collecting them here and then its dynamically adjusting.
    /// <summary>
    /// The thrusters responsible to MOVE the ship FORWARD.
    /// </summary>
    [SerializeField, Header("Linear Thruster")]
    Thruster[] forwardThrusters;

    /// <summary>
    /// The thrusters responsible to MOVE the ship BACKWARDS.
    /// </summary>
    [SerializeField]
    Thruster[] backwardThrusters;

    /// <summary>
    /// The thrusters responsible to MOVE the ship LEFT.
    /// </summary>
    [SerializeField]
    Thruster[] leftThrusters;

    /// <summary>
    /// The thrusters responsible to MOVE the ship RIGHT.
    /// </summary>
    [SerializeField]
    Thruster[] rightThrusters;

    /// <summary>
    /// The thrusters responsible to MOVE the ship UP.
    /// </summary>
    [SerializeField]
    Thruster[] upThrusters;

    /// <summary>
    /// The thrusters responsible to MOVE the ship DOWN.
    /// </summary>
    [SerializeField]
    Thruster[] downThrusters;

    /// <summary>
    /// The thrusters responsible to ROTATE the ship NOSE UP.
    /// </summary>
    [SerializeField, Header("Rotational Thruster")]
    Thruster[] pitchUpThrusters;

    /// <summary>
    /// The thrusters responsible to ROTATE the ship NOSE DOWN.
    /// </summary>
    [SerializeField]
    Thruster[] pitchDownThrusters;

    /// <summary>
    /// The thrusters responsible to ROTATE the ship NOSE LEFT.
    /// </summary>
    [SerializeField]
    Thruster[] yawLeftThrusters;

    /// <summary>
    /// The thrusters responsible to ROTATE the ship NOSE RIGHT.
    /// </summary>
    [SerializeField]
    Thruster[] yawRightThrusters;

    /// <summary>
    /// The thrusters responsible to ROTATE (ROLL) the ship COUNTER CLOCKWISE.
    /// </summary>
    [SerializeField]
    Thruster[] rollCounterClockwiseThrusters;

    /// <summary>
    /// The thrusters responsible to ROTATE (ROLL) the ship CLOCKWISE.
    /// </summary>
    [SerializeField]
    Thruster[] rollClockwiseThrusters;

    private InputHandler inputHandler;

    float forceAmount = 100;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ResetAllThrusters();
        Vector3 currentInputVector = inputHandler.GetMoveInputVector();
        Vector3 currentLookVector = inputHandler.GetLookInputVector();

        LinearThrust(currentInputVector);
        RotationalThrust(currentLookVector);

        Vector3 linearVelocity = rb.linearVelocity;
        Vector3 angularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
        // print(linearVelocity);
        LinearInertiaDampening(currentInputVector, linearVelocity);
        THRUSTERTEMP(leftThrusters[0]);
        // Debug.DrawLine(rb.transform.position, rb.transform.position + linearVelocity);
    }


    void LinearThrust(Vector3 direction)
    {
        // forward and backwards
        if (direction.z > 0)
        {
            AddThrustToThrusterGroup(ref forwardThrusters, forceAmount);
        }
        else if (direction.z < 0)
        {
            AddThrustToThrusterGroup(ref backwardThrusters, forceAmount);
        }


        // left and right
        if (direction.x > 0)
        {
            AddThrustToThrusterGroup(ref rightThrusters, forceAmount);
        }
        else if (direction.x < 0)
        {
            AddThrustToThrusterGroup(ref leftThrusters, forceAmount);
        }


        // up and down
        if (direction.y > 0)
        {
            AddThrustToThrusterGroup(ref upThrusters, forceAmount);
        }
        else if (direction.y < 0)
        {
            AddThrustToThrusterGroup(ref downThrusters, forceAmount);
        }
    }

    // PC controls
    void RotationalThrust(Vector3 look)
    {
        // right and left (PC Only)
        if (look.x > 0.1f)
        {
            float amount = Mathf.Abs(look.x);

            PulseThrusterGroup(ref yawRightThrusters, forceAmount * amount * Time.deltaTime);
        }
        else if (look.x < -0.1f)
        {
            float amount = Mathf.Abs(look.x);

            PulseThrusterGroup(ref yawLeftThrusters, forceAmount * amount * Time.deltaTime);
        }


        // up and down (PC Only)
        if (look.y > 0.1f)
        {
            float amount = Mathf.Abs(look.y);

            PulseThrusterGroup(ref pitchUpThrusters, forceAmount * amount * Time.deltaTime);
        }
        else if (look.y < -0.1f)
        {
            float amount = Mathf.Abs(look.y);

            PulseThrusterGroup(ref pitchDownThrusters, forceAmount * amount * Time.deltaTime);
        }


        // Q and E PC and controller universal.
        // Roll right and left
        if (look.z > 0)
        {
            AddThrustToThrusterGroup(ref rollClockwiseThrusters, forceAmount);
        }
        else if (look.z < 0)
        {
            AddThrustToThrusterGroup(ref rollCounterClockwiseThrusters, forceAmount);
        }
    }

    bool IsCloseToZero(float axis)
    {
        return axis < 0.1f && axis > -0.1f;
    }

    void RotationalInertiaDampening(Vector3 inputLookVector, Vector3 currentAngularVelocity)
    {

    }

    void THRUSTERTEMP(Thruster thruster)
    {
        Vector3 displacement = thruster.transform.localPosition - rb.centerOfMass;
        Vector3 forceVector = thruster.GetForceDirection() * forceAmount;
        Vector3 torque = Vector3.Cross(displacement, forceVector);
        // print(torque);
        float inertia = rb.mass * Mathf.Pow(displacement.magnitude, 2f);

        // average out the torque from all thrusters. here before the final calc.
        // Res is in rads and NOT in degrees.
        Vector3 angularDisplacement = torque / inertia;

        print(angularDisplacement);
    }

    void LinearInertiaDampening(Vector3 inputVector, Vector3 currentLinearVelocity)
    {
        // Vector3 targetDampen = -currentLinearVelocity;
        //Vector3 dampenDirection = (inputVector.normalized - currentLinearVelocity.normalized).normalized;

        Vector3 GetDampenDirection(Vector3 inputV, Vector3 currentLVelocity)
        {
            currentLVelocity.Normalize();

            Vector3 finalVector = Vector3.zero;

            if (IsCloseToZero(inputV.x))
            {
                finalVector.x = -currentLVelocity.x;
            }

            if (IsCloseToZero(inputV.y))
            {
                finalVector.y = -currentLVelocity.y;
            }

            if (IsCloseToZero(inputV.z))
            {
                finalVector.z = -currentLVelocity.z;
            }

            return finalVector;
        }

        Vector3 velocityInLocal = rb.transform.InverseTransformVector(currentLinearVelocity);
        // Debug.DrawLine(rb.transform.position, rb.transform.position + rb.transform.TransformVector(velocityInLocal), Color.red);
        //print($"{velocityInLocal.normalized} vs {currentLinearVelocity.normalized}");

        Vector3 accelVector = GetAccelVector(GetDampenDirection(inputVector, velocityInLocal));
        Vector3 finalVelocity = currentLinearVelocity + (accelVector * Time.deltaTime);


        // return;

        if (IsCloseToZero(inputVector.x))
        {
            if (velocityInLocal.x > 0)
            {
                // if (finalVelocity.x < 0)
                // {
                //     // overshot
                //     // to get the force required, reverse the formula. Get the accel, then plug that in to get the force needed.
                // }
                // else if (finalVelocity.x > 0)
                // {
                //     AddThrustToThrusterGroup(ref rightThrusters, forceAmount);
                // }
                AddThrustToThrusterGroup(ref leftThrusters, forceAmount);

            }
            else if (velocityInLocal.x < 0)
            {

                // if (finalVelocity.x > 0)
                // {
                //     // overshot
                // }
                // else if (finalVelocity.x < 0)
                // {
                //     AddThrustToThrusterGroup(ref leftThrusters, forceAmount);
                // }
                AddThrustToThrusterGroup(ref rightThrusters, forceAmount);

            }
        }

        if (IsCloseToZero(inputVector.y))
        {
            if (velocityInLocal.y > 0)
            {
                // if (finalVelocity.y < 0)
                // {
                //     // overshot
                //     // to get the force required, reverse the formula. Get the accel, then plug that in to get the force needed.
                // }
                // else if (finalVelocity.y > 0)
                // {
                //     AddThrustToThrusterGroup(ref upThrusters, forceAmount);
                // }

                AddThrustToThrusterGroup(ref downThrusters, forceAmount);

            }
            else if (velocityInLocal.y < 0)
            {

                // if (finalVelocity.y > 0)
                // {
                //     // overshot
                // }
                // else if (finalVelocity.y < 0)
                // {
                //     AddThrustToThrusterGroup(ref downThrusters, forceAmount);
                // }
                AddThrustToThrusterGroup(ref upThrusters, forceAmount);

            }
        }

        if (IsCloseToZero(inputVector.z))
        {
            if (velocityInLocal.z > 0)
            {
                // if (finalVelocity.z < 0)
                // {
                //     // overshot
                //     // to get the force required, reverse the formula. Get the accel, then plug that in to get the force needed.
                // }
                // else if (finalVelocity.z > 0)
                // {
                //     AddThrustToThrusterGroup(ref forwardThrusters, forceAmount);
                // }
                AddThrustToThrusterGroup(ref backwardThrusters, forceAmount);

            }
            else if (velocityInLocal.z < 0)
            {

                // if (finalVelocity.z > 0)
                // {
                //     // overshot
                // }
                // else if (finalVelocity.z < 0)
                // {
                //     AddThrustToThrusterGroup(ref backwardThrusters, forceAmount);
                // }
                AddThrustToThrusterGroup(ref forwardThrusters, forceAmount);

            }
        }

        // if (currentLinearVelocity.z > 0)
        // {
        //     // float accel = (GetAllThrusterForce(ref forwardThrusters) / rb.mass) * Time.deltaTime;

        //     // if (currentLinearVelocity.z + accel)
        //     //     float zRes = currentLinearVelocity.z + accel;

        // }
    }

    Vector3 GetAccelVector(Vector3 neededDirections)
    {
        neededDirections.Normalize();
        Vector3 accelVector = Vector3.zero;

        // to move right we need our left thrusters.

        if (neededDirections.z > 0)
        {
            accelVector.z = GetAllThrusterForce(ref backwardThrusters) / rb.mass;
        }
        else if (neededDirections.z < 0)
        {
            accelVector.z = -(GetAllThrusterForce(ref forwardThrusters) / rb.mass);
        }

        if (neededDirections.x > 0)
        {
            accelVector.x = GetAllThrusterForce(ref leftThrusters) / rb.mass;
        }
        else if (neededDirections.x < 0)
        {
            accelVector.x = -(GetAllThrusterForce(ref rightThrusters) / rb.mass);
        }

        if (neededDirections.y > 0)
        {
            accelVector.y = GetAllThrusterForce(ref downThrusters) / rb.mass;
        }
        else if (neededDirections.y < 0)
        {
            accelVector.y = -(GetAllThrusterForce(ref upThrusters) / rb.mass);
        }

        return accelVector;
    }

    float GetAllThrusterForce(ref Thruster[] thrusters)
    {
        float collectedForce = 0f;

        foreach (var thruster in thrusters)
        {
            if (!thruster.IsComponentDestroyed())
            {
                collectedForce += forceAmount;
            }
        }

        return collectedForce;
    }

    void ResetAllThrusters()
    {
        // linear thrusters
        ResetThrusterGroup(ref forwardThrusters);
        ResetThrusterGroup(ref backwardThrusters);
        ResetThrusterGroup(ref leftThrusters);
        ResetThrusterGroup(ref rightThrusters);
        ResetThrusterGroup(ref upThrusters);
        ResetThrusterGroup(ref downThrusters);

        // rotational thrusters
        ResetThrusterGroup(ref pitchUpThrusters);
        ResetThrusterGroup(ref pitchDownThrusters);
        ResetThrusterGroup(ref yawLeftThrusters);
        ResetThrusterGroup(ref yawRightThrusters);
        ResetThrusterGroup(ref rollCounterClockwiseThrusters);
        ResetThrusterGroup(ref rollClockwiseThrusters);
    }

    void ResetThrusterGroup(ref Thruster[] thrusters)
    {
        foreach (var thruster in thrusters)
        {
            thruster.SetThrusterForce(0);
        }
    }

    void AddThrustToThrusterGroup(ref Thruster[] thrusters, float forceToAdd)
    {
        foreach (var thruster in thrusters)
        {
            thruster.AddToThrusterForce(forceToAdd);
        }
    }

    void PulseThrusterGroup(ref Thruster[] thrusters, float force)
    {
        foreach (var thruster in thrusters)
        {
            thruster.PulseThruster(force);
        }
    }

    Vector3 ClampVector(Vector3 vector)
    {
        const int LOWER_BOUND = -1;
        const int UPPER_BOUND = 1;
        return new Vector3(Mathf.Clamp(vector.x, LOWER_BOUND, UPPER_BOUND), Mathf.Clamp(vector.y, LOWER_BOUND, UPPER_BOUND), Mathf.Clamp(vector.z, LOWER_BOUND, UPPER_BOUND));
    }
}
