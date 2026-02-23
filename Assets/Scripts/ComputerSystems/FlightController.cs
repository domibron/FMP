using UnityEngine;

public class FlightController : MonoBehaviour
{
    // forward facing thrusters (next to cockpit)
    // Could replace with getting thruster direction instead then collecting them here and then its dynamically adjusting.
    [SerializeField, Header("Linear Thruster")]
    Thruster[] forwardThrusters;

    [SerializeField]
    Thruster[] backwardThrusters;

    [SerializeField]
    Thruster[] leftThrusters;

    [SerializeField]
    Thruster[] rightThrusters;

    [SerializeField]
    Thruster[] upThrusters;

    [SerializeField]
    Thruster[] downThrusters;

    [SerializeField, Header("Rotational Thruster")]
    Thruster[] pitchUpThrusters;

    [SerializeField]
    Thruster[] pitchDownThrusters;

    [SerializeField]
    Thruster[] yawLeftThrusters;

    [SerializeField]
    Thruster[] yawRightThrusters;

    [SerializeField]
    Thruster[] rollCounterClockwiseThrusters;

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

        Vector3 linearVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        Vector3 angularVelocity = transform.InverseTransformDirection(rb.angularVelocity);


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
        else if (look.x < 0.1f)
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
        else if (look.y < 0.1f)
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
