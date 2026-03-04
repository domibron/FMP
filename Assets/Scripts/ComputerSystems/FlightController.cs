using System;
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
        // print(DateTime.Now.ToString());
        // print(linearVelocity);
        LinearInertiaDampening(currentInputVector, linearVelocity);
        RotationalInertiaDampening(currentLookVector, angularVelocity);
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
        // Vector3 velocityInLocal = Quaternion.Inverse(rb.transform.rotation) * currentAngularVelocity;
        Vector3 velocityInLocal = currentAngularVelocity;

        Vector3 convertedInputToRotationalVector = new Vector3(-inputLookVector.y, inputLookVector.x, -inputLookVector.z);

        Vector3 angularVelocity = GetAngularDisplacement(-GetIgnoredAxis(convertedInputToRotationalVector, velocityInLocal.normalized));

        Vector3 finalVelocity = currentAngularVelocity + (angularVelocity * Time.deltaTime);

        // print(angularVelocity + " " + currentAngularVelocity);

        // print(currentAngularVelocity);

        // return;
        if (IsCloseToZero(inputLookVector.x)) // YAW
        {
            if (velocityInLocal.y > 0) // YAW Right
            {
                if (finalVelocity.y < 0)
                {
                    // overshot
                    // to get the force required, reverse the formula. Get the acceleration, then plug that in to get the force needed.
                }
                else if (finalVelocity.y > 0)
                {
                    AddThrustToThrusterGroup(ref yawLeftThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref leftThrusters, forceAmount);

            }
            else if (velocityInLocal.y < 0) // YAW Left
            {

                if (finalVelocity.y > 0)
                {
                    // overshot
                }
                else if (finalVelocity.y < 0)
                {
                    AddThrustToThrusterGroup(ref yawRightThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref rightThrusters, forceAmount);

            }
        }

        if (IsCloseToZero(inputLookVector.y)) // PITCH
        {
            if (velocityInLocal.x > 0) // PITCH Down
            {
                if (finalVelocity.x < 0)
                {
                    // overshot
                    // to get the force required, reverse the formula. Get the acceleration, then plug that in to get the force needed.
                }
                else if (finalVelocity.x > 0)
                {
                    AddThrustToThrusterGroup(ref pitchUpThrusters, forceAmount);
                }

                // AddThrustToThrusterGroup(ref downThrusters, forceAmount);

            }
            else if (velocityInLocal.x < 0) // PITCH Up
            {

                if (finalVelocity.x > 0)
                {
                    // overshot
                }
                else if (finalVelocity.x < 0)
                {
                    AddThrustToThrusterGroup(ref pitchDownThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref upThrusters, forceAmount);

            }
        }

        if (IsCloseToZero(inputLookVector.z)) // ROLL
        {
            if (velocityInLocal.z > 0) // ROLL Counter Wise
            {
                if (finalVelocity.z < 0)
                {
                    // overshot
                    // to get the force required, reverse the formula. Get the acceleration, then plug that in to get the force needed.
                }
                else if (finalVelocity.z > 0)
                {
                    AddThrustToThrusterGroup(ref rollClockwiseThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref backwardThrusters, forceAmount);

            }
            else if (velocityInLocal.z < 0) // ROLL Wise
            {

                if (finalVelocity.z > 0)
                {
                    // overshot
                }
                else if (finalVelocity.z < 0)
                {
                    AddThrustToThrusterGroup(ref rollCounterClockwiseThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref forwardThrusters, forceAmount);

            }
        }

        //print($"{GetCombinedAngularDisplacement(ref pitchDownThrusters)} {GetCombinedAngularDisplacement(ref rollClockwiseThrusters)} {GetCombinedAngularDisplacement(ref yawLeftThrusters)}");
    }

    Vector3 GetAngularAcceleration(ref Thruster[] thrusters)
    {
        // ^ name is technically wrong.

        Vector3 combinedTorque = Vector3.zero;

        foreach (Thruster thr in thrusters)
        {
            combinedTorque += GetTorque(thr);
        }
        // is this wrong?
        // return combinedTorque / (rb.mass * Mathf.Pow(5f, 2f));
        return combinedTorque; // / (rb.mass * Mathf.Pow(5f, 2f));
    }

    Vector3 GetTorque(Thruster thruster)
    {
        Vector3 displacement = thruster.transform.localPosition - rb.centerOfMass;
        Vector3 forceVector = thruster.GetForceDirection() * forceAmount;
        Vector3 torque = Vector3.Cross(displacement, forceVector);
        // print(torque);
        float inertia = rb.mass * Mathf.Pow(displacement.magnitude, 2f);

        // average out the torque from all thrusters. here before the final calc.
        // Res is in rads and NOT in degrees. ~ rads as in radiation? ~ no, as in radians!
        // Vector3 angularDisplacement = torque / inertia;

        // Vector3 angularVelocity = angularDisplacement * Time.deltaTime;
        // return angularDisplacement * Time.deltaTime;

        // return torque;
        return torque / inertia;

        // print(angularDisplacement);
    }

    Vector3 GetAngularDisplacement(Vector3 desiredRotationDirections)
    {
        Vector3 finalAngularAcceleration = Vector3.zero;

        if (desiredRotationDirections.x > 0)
        {
            finalAngularAcceleration += GetAngularAcceleration(ref pitchDownThrusters);
        }
        else if (desiredRotationDirections.x < 0)
        {
            finalAngularAcceleration += GetAngularAcceleration(ref pitchUpThrusters);
        }


        if (desiredRotationDirections.y > 0)
        {
            finalAngularAcceleration += GetAngularAcceleration(ref yawRightThrusters);
        }
        else if (desiredRotationDirections.y < 0)
        {
            finalAngularAcceleration += GetAngularAcceleration(ref yawLeftThrusters);
        }

        if (desiredRotationDirections.z > 0)
        {
            finalAngularAcceleration += GetAngularAcceleration(ref rollClockwiseThrusters);
        }
        else if (desiredRotationDirections.z < 0)
        {
            finalAngularAcceleration += GetAngularAcceleration(ref rollCounterClockwiseThrusters);
        }

        return finalAngularAcceleration;
    }

    Vector3 GetIgnoredAxis(Vector3 inputV, Vector3 currentVector)
    {
        // currentVector.Normalize();

        Vector3 finalVector = Vector3.zero;

        if (IsCloseToZero(inputV.x))
        {
            finalVector.x = -currentVector.x;
        }

        if (IsCloseToZero(inputV.y))
        {
            finalVector.y = -currentVector.y;
        }

        if (IsCloseToZero(inputV.z))
        {
            finalVector.z = -currentVector.z;
        }

        return finalVector;
    }

    void LinearInertiaDampening(Vector3 inputVector, Vector3 currentLinearVelocity)
    {
        // Vector3 targetDampen = -currentLinearVelocity;
        //Vector3 dampenDirection = (inputVector.normalized - currentLinearVelocity.normalized).normalized;



        // Vector3 velocityInLocal = rb.transform.InverseTransformVector(currentLinearVelocity);
        Vector3 velocityInLocal = rb.transform.InverseTransformDirection(currentLinearVelocity);
        // Debug.DrawLine(rb.transform.position, rb.transform.position + rb.transform.TransformVector(velocityInLocal), Color.red);
        //print($"{velocityInLocal.normalized} vs {currentLinearVelocity.normalized}");

        // print(currentLinearVelocity);

        Vector3 accelerationVector = GetAccelerationVector(GetIgnoredAxis(inputVector, velocityInLocal.normalized));
        Vector3 finalVelocity = velocityInLocal + (accelerationVector * Time.deltaTime);

        // print(velocityInLocal);

        // return;

        if (IsCloseToZero(inputVector.x))
        {
            if (velocityInLocal.x > 0) // Right
            {
                if (finalVelocity.x < 0)
                {
                    // overshot
                    // to get the force required, reverse the formula. Get the acceleration, then plug that in to get the force needed.
                }
                else if (finalVelocity.x > 0)
                {
                    AddThrustToThrusterGroup(ref leftThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref leftThrusters, forceAmount);

            }
            else if (velocityInLocal.x < 0) // left
            {

                if (finalVelocity.x > 0)
                {
                    // overshot
                }
                else if (finalVelocity.x < 0)
                {
                    AddThrustToThrusterGroup(ref rightThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref rightThrusters, forceAmount);

            }
        }

        if (IsCloseToZero(inputVector.y))
        {
            if (velocityInLocal.y > 0) // up
            {
                if (finalVelocity.y < 0)
                {
                    // overshot
                    // to get the force required, reverse the formula. Get the acceleration, then plug that in to get the force needed.
                }
                else if (finalVelocity.y > 0)
                {
                    AddThrustToThrusterGroup(ref downThrusters, forceAmount);
                }

                // AddThrustToThrusterGroup(ref downThrusters, forceAmount);

            }
            else if (velocityInLocal.y < 0) // down
            {

                if (finalVelocity.y > 0)
                {
                    // overshot
                }
                else if (finalVelocity.y < 0)
                {
                    AddThrustToThrusterGroup(ref upThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref upThrusters, forceAmount);

            }
        }

        if (IsCloseToZero(inputVector.z))
        {
            if (velocityInLocal.z > 0) // forward
            {
                if (finalVelocity.z < 0)
                {
                    // overshot
                    // to get the force required, reverse the formula. Get the acceleration, then plug that in to get the force needed.
                }
                else if (finalVelocity.z > 0)
                {
                    AddThrustToThrusterGroup(ref backwardThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref backwardThrusters, forceAmount);

            }
            else if (velocityInLocal.z < 0) // backwards
            {

                if (finalVelocity.z > 0)
                {
                    // overshot
                }
                else if (finalVelocity.z < 0)
                {
                    AddThrustToThrusterGroup(ref forwardThrusters, forceAmount);
                }
                // AddThrustToThrusterGroup(ref forwardThrusters, forceAmount);

            }
        }

        // if (currentLinearVelocity.z > 0)
        // {
        //     // float acceleration = (GetAllThrusterForce(ref forwardThrusters) / rb.mass) * Time.deltaTime;

        //     // if (currentLinearVelocity.z + acceleration)
        //     //     float zRes = currentLinearVelocity.z + acceleration;

        // }
    }

    Vector3 GetAccelerationVector(Vector3 neededDirections)
    {
        neededDirections.Normalize();
        Vector3 accelerationVector = Vector3.zero;

        // to move right we need our left thrusters.

        if (neededDirections.z > 0)
        {
            accelerationVector.z = GetAllThrusterForce(ref backwardThrusters) / rb.mass;
        }
        else if (neededDirections.z < 0)
        {
            accelerationVector.z = -(GetAllThrusterForce(ref forwardThrusters) / rb.mass);
        }

        if (neededDirections.x > 0)
        {
            accelerationVector.x = GetAllThrusterForce(ref leftThrusters) / rb.mass;
        }
        else if (neededDirections.x < 0)
        {
            accelerationVector.x = -(GetAllThrusterForce(ref rightThrusters) / rb.mass);
        }

        if (neededDirections.y > 0)
        {
            accelerationVector.y = GetAllThrusterForce(ref downThrusters) / rb.mass;
        }
        else if (neededDirections.y < 0)
        {
            accelerationVector.y = -(GetAllThrusterForce(ref upThrusters) / rb.mass);
        }

        return accelerationVector;
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
