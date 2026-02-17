using UnityEngine;

public class FlightController : MonoBehaviour
{
    // forward facing thrusters (next to cockpit)
    // Could replace with getting thruster direction instead then collecting them here and then its dynamically adjusting.
    [SerializeField]
    Thruster frontLeft;
    [SerializeField]
    Thruster frontRight;

    [SerializeField]
    Thruster rearLeft;
    [SerializeField]
    Thruster rearRight;

    [SerializeField]
    Thruster leftFront;
    [SerializeField]
    Thruster leftRear;

    [SerializeField]
    Thruster rightFront;
    [SerializeField]
    Thruster rightRear;

    [SerializeField]
    Thruster topFrontLeft;
    [SerializeField]
    Thruster topRearLeft;
    [SerializeField]
    Thruster topFrontRight;
    [SerializeField]
    Thruster topRearRight;

    [SerializeField]
    Thruster bottomFrontLeft;
    [SerializeField]
    Thruster bottomRearLeft;
    [SerializeField]
    Thruster bottomFrontRight;
    [SerializeField]
    Thruster bottomRearRight;


    float frontLeftThrust;
    float frontRightThrust;

    float rearLeftThrust;
    float rearRightThrust;

    float leftFrontThrust;
    float leftRearThrust;

    float rightFrontThrust;
    float rightRearThrust;

    float topFrontLeftThrust;
    float topRearLeftThrust;
    float topFrontRightThrust;
    float topRearRightThrust;

    float bottomFrontLeftThrust;
    float bottomRearLeftThrust;
    float bottomFrontRightThrust;
    float bottomRearRightThrust;

    float forceAmount = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        print(GetLookAsVector().ToString());
        ResetThrust();
        LinearThrust(GetInputAsVector());
        RotationalThrust(GetLookAsVector());
        ApplyThrust();
    }

    // TODO: replace with new input system.
    Vector3 GetInputAsVector()
    {
        Vector3 inputVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputVector.z += 1;
        if (Input.GetKey(KeyCode.S)) inputVector.z -= 1;

        if (Input.GetKey(KeyCode.D)) inputVector.x += 1;
        if (Input.GetKey(KeyCode.A)) inputVector.x -= 1;

        if (Input.GetKey(KeyCode.Space)) inputVector.y += 1;
        if (Input.GetKey(KeyCode.LeftControl)) inputVector.y -= 1;

        return inputVector;
    }

    // TODO: replace with new input system.
    Vector3 GetLookAsVector()
    {
        Vector3 inputVector = Vector3.zero;
        if (Input.GetKey(KeyCode.E)) inputVector.z += 1;
        if (Input.GetKey(KeyCode.Q)) inputVector.z -= 1;

        inputVector.x = Input.GetAxisRaw("Mouse X");
        inputVector.y = Input.GetAxisRaw("Mouse Y");

        return inputVector;
    }

    void LinearThrust(Vector3 direction)
    {
        if (direction.z > 0)
        {
            rearLeftThrust += forceAmount;
            rearRightThrust += forceAmount;
        }
        else if (direction.z < 0)
        {
            frontLeftThrust += forceAmount;
            frontRightThrust += forceAmount;
        }
        // else
        // {
        //     // It will reset.
        // }

        if (direction.x > 0)
        {
            leftFrontThrust += forceAmount;
            leftRearThrust += forceAmount;
        }
        else if (direction.x < 0)
        {
            rightFrontThrust += forceAmount;
            rightRearThrust += forceAmount;
        }

        if (direction.y > 0)
        {
            bottomFrontLeftThrust += forceAmount;
            bottomRearLeftThrust += forceAmount;
            bottomFrontRightThrust += forceAmount;
            bottomRearRightThrust += forceAmount;
        }
        else if (direction.y < 0)
        {
            topFrontLeftThrust += forceAmount;
            topRearLeftThrust += forceAmount;
            topFrontRightThrust += forceAmount;
            topRearRightThrust += forceAmount;
        }
    }

    // PC controls
    void RotationalThrust(Vector3 look)
    {
        if (look.x > 0.1f) // right and left
        {
            float amount = Mathf.Abs(look.x);
            rearLeft.PulseThruster(forceAmount * amount * Time.deltaTime);
            frontRight.PulseThruster(forceAmount * amount * Time.deltaTime);

            leftFront.PulseThruster(forceAmount * amount * Time.deltaTime);
            rightRear.PulseThruster(forceAmount * amount * Time.deltaTime);
        }
        else if (look.x < 0.1f)
        {
            float amount = Mathf.Abs(look.x);
            rearRight.PulseThruster(forceAmount * amount * Time.deltaTime);
            frontLeft.PulseThruster(forceAmount * amount * Time.deltaTime);

            leftRear.PulseThruster(forceAmount * amount * Time.deltaTime);
            rightFront.PulseThruster(forceAmount * amount * Time.deltaTime);
        }
        // else
        // {
        //     // It will reset.
        // }

        if (look.y > 0.1f) // up and down
        {
            float amount = Mathf.Abs(look.y);
            bottomFrontLeft.PulseThruster(forceAmount * amount * Time.deltaTime);
            bottomFrontRight.PulseThruster(forceAmount * amount * Time.deltaTime);

            topRearLeft.PulseThruster(forceAmount * amount * Time.deltaTime);
            topRearRight.PulseThruster(forceAmount * amount * Time.deltaTime);
        }
        else if (look.y < 0.1f)
        {
            float amount = Mathf.Abs(look.y);
            bottomRearLeft.PulseThruster(forceAmount * amount * Time.deltaTime);
            bottomRearRight.PulseThruster(forceAmount * amount * Time.deltaTime);

            topFrontLeft.PulseThruster(forceAmount * amount * Time.deltaTime);
            topFrontRight.PulseThruster(forceAmount * amount * Time.deltaTime);
        }

        // Q and E PC and controller universal.
        if (look.z > 0) // Roll right and left
        {
            bottomFrontLeftThrust += forceAmount;
            bottomRearLeftThrust += forceAmount;

            topFrontRightThrust += forceAmount;
            topRearRightThrust += forceAmount;
        }
        else if (look.z < 0)
        {
            bottomFrontRightThrust += forceAmount;
            bottomRearRightThrust += forceAmount;

            topFrontLeftThrust += forceAmount;
            topRearLeftThrust += forceAmount;
        }
    }

    void ResetThrust()
    {
        frontLeftThrust = 0;
        frontRightThrust = 0;

        rearLeftThrust = 0;
        rearRightThrust = 0;

        leftFrontThrust = 0;
        leftRearThrust = 0;

        rightFrontThrust = 0;
        rightRearThrust = 0;

        topFrontLeftThrust = 0;
        topRearLeftThrust = 0;
        topFrontRightThrust = 0;
        topRearRightThrust = 0;

        bottomFrontLeftThrust = 0;
        bottomRearLeftThrust = 0;
        bottomFrontRightThrust = 0;
        bottomRearRightThrust = 0;
    }

    void ApplyThrust()
    {
        frontLeft.SetThrusterForce(frontLeftThrust);
        frontRight.SetThrusterForce(frontRightThrust);

        rearLeft.SetThrusterForce(rearLeftThrust);
        rearRight.SetThrusterForce(rearRightThrust);

        leftFront.SetThrusterForce(leftFrontThrust);
        leftRear.SetThrusterForce(leftRearThrust);

        rightFront.SetThrusterForce(rightFrontThrust);
        rightRear.SetThrusterForce(rightRearThrust);

        topFrontLeft.SetThrusterForce(topFrontLeftThrust);
        topRearLeft.SetThrusterForce(topRearLeftThrust);
        topFrontRight.SetThrusterForce(topFrontRightThrust);
        topRearRight.SetThrusterForce(topRearRightThrust);

        bottomFrontLeft.SetThrusterForce(bottomFrontLeftThrust);
        bottomRearLeft.SetThrusterForce(bottomRearLeftThrust);
        bottomFrontRight.SetThrusterForce(bottomFrontRightThrust);
        bottomRearRight.SetThrusterForce(bottomRearRightThrust);
    }
}
