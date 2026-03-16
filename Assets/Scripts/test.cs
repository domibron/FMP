using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    float forceAmount = 100f;

    [SerializeField]
    Thruster thruster;

    [SerializeField, Range(-1, 1)]
    float x = 0;

    [SerializeField, Range(-1, 1)]
    float y = 0;

    float maxAngle = 80;

    [SerializeField]
    Transform thrusterY;

    [SerializeField]
    Transform target;

    const float MAX_ANG_VEL = 30f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetInLocal = target.position - rb.transform.position;

        Vector3 linierVelToCancel = (targetInLocal - rb.linearVelocity) - rb.linearVelocity;


        // * DEBUG Visual
        Debug.DrawLine(rb.transform.position, rb.transform.position + linierVelToCancel, Color.blue);
        Debug.DrawLine(rb.transform.position, rb.transform.position + (targetInLocal), Color.red);
        Debug.DrawLine(rb.transform.position, rb.transform.position + rb.linearVelocity, Color.green);


        Vector3 rotationAngle = GetRotationAtoB(rb.transform.forward, target.position - rb.transform.position);
        Vector3 velAngle = GetRotationAtoB(rb.transform.forward, linierVelToCancel);
        Vector3 CounterLin = GetRotationAtoB(rb.transform.forward, -rb.linearVelocity);


        Vector3 finalRot = (velAngle + rotationAngle + CounterLin) / 3f;
        Debug.DrawLine(rb.transform.position, rb.transform.position + (Quaternion.Euler(finalRot) * rb.transform.forward * 3f), Color.orange);

        Vector3 localAngularVel = rb.transform.InverseTransformDirection(rb.angularVelocity);
        // Calculate the deaccel.

        // TODO: need to calulate the deaccel and use that against the angular accel to know when we need to start slowing down.
        Vector3 neededAngularChange = finalRot;// - (localAngularVel * Mathf.Rad2Deg);
        Vector2 angularCounterInputRot = new Vector2(neededAngularChange.y, -neededAngularChange.x);

        AngleThrusterToTarget(angularCounterInputRot.normalized * 1f);
        Vector3 torque = GetTorque(thruster);

        float magNeed = (angularCounterInputRot.magnitude / (torque.magnitude * Time.fixedDeltaTime));
        // print(angle + " " + (torque.magnitude * Time.fixedDeltaTime) + "  -  " + magNeed);
        // print(magNeed);
        angularCounterInputRot = angularCounterInputRot.normalized * magNeed;




        // Vector3 

        AngleThrusterToTarget(angularCounterInputRot);

        // float AngleAgressive = (angle * Mathf.Rad2Deg) / maxAngle;
        // Vector3 diff = (rotationAngle).normalized - localAngularVel.normalized;
        // Vector2 input = new Vector3(diff.y, -diff.x).normalized * AngleAgressive;

        // AngleThrusterToTarget(input);

        // print(input + " - " + diff + "  , " + rotationAngle.normalized + " . " + localAngularVel.normalized);


        // Vector3 rot = GetTorque(thruster);
        // print("torque = " + rot);
    }

    private Vector3 GetRotationAtoBLocal(Vector3 dirA, Vector3 dirB)
    {
        dirA.Normalize();
        dirB.Normalize();

        Vector3 angleAxisAngular = Vector3.Cross(dirA, dirB);
        angleAxisAngular = rb.transform.InverseTransformDirection(angleAxisAngular);


        float angle = (Mathf.Acos(Vector3.Dot(dirA, dirB))) / (dirB.magnitude * dirA.magnitude);
        // print("angle = " + angle);
        return angleAxisAngular * angle;// in rads, also angular vel is in rads too.
    }

    private Vector3 GetRotationAtoB(Vector3 dirA, Vector3 dirB)
    {
        dirA.Normalize();
        dirB.Normalize();

        Vector3 angleAxisAngular = Vector3.Cross(dirA, dirB);
        // angleAxisAngular = rb.transform.InverseTransformDirection(angleAxisAngular);


        float angle = (Mathf.Acos(Vector3.Dot(dirA, dirB))) / (dirB.magnitude * dirA.magnitude);
        // print("angle = " + angle);
        return angleAxisAngular * (angle * Mathf.Rad2Deg);// in rads, also angular vel is in rads too.
    }

    private Vector2 GetInputVector(Vector3 targetPosWorld)
    {
        // const float MAX_TURN_RATE = 30f;

        Vector3 directionToTarget = rb.transform.InverseTransformDirection(targetPosWorld - rb.transform.position);

        float XDot = Vector3.Dot(Vector3.right.normalized, directionToTarget.normalized);
        float YDot = Vector3.Dot(Vector3.up.normalized, directionToTarget.normalized);

        float XAngleNeeded = Mathf.Asin(XDot) * Mathf.Rad2Deg;
        float YAngleNeeded = Mathf.Asin(YDot) * Mathf.Rad2Deg;

        print(XAngleNeeded + " " + YAngleNeeded);

        Vector3 angularRotation = rb.transform.InverseTransformDirection(rb.angularVelocity);

        float XRotationalVel = -angularRotation.y;
        float YRotationalVel = angularRotation.x;

        if (Mathf.Abs(XAngleNeeded) < 0.1f) print("FUCK");

        // float xFinal = (Mathf.Asin(XDot) * Mathf.Rad2Deg) / maxAngle;
        // float yFinal = (Mathf.Asin(YDot) * Mathf.Rad2Deg) / maxAngle;

        float xFinal = (XAngleNeeded + XRotationalVel) / maxAngle;
        float yFinal = (YAngleNeeded + YRotationalVel) / maxAngle;


        return new Vector2(xFinal, yFinal);
    }

    private void AngleThrusterToTarget(Vector2 input)
    {
        input = -input; // invert input because pitching the thruster up will rotate the missile up.

        // get the Atan2 angle of displacement from 0, get the mag for the angle deviation from 0,0 aka back.

        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        if (input.magnitude > 1f) input.Normalize();

        float mag = input.magnitude;

        thrusterY.transform.localRotation = Quaternion.Euler(0, 0, angle);

        thruster.transform.localRotation = Quaternion.Euler(0, mag * maxAngle, 0);
    }

    Vector3 GetTorque(Thruster thruster)
    {
        Vector3 displacement = thruster.transform.position - (rb.transform.position + rb.centerOfMass);
        Vector3 forceVector = (Quaternion.Inverse(thrusterY.transform.localRotation) * Quaternion.Inverse(thruster.transform.localRotation) * thruster.GetForceDirectionVector().normalized) * forceAmount;
        Vector3 torque = Vector3.Cross(displacement, forceVector);
        // print(torque);
        float inertia = rb.mass * Mathf.Pow(displacement.magnitude, 2f);

        // average out the torque from all thrusters. here before the final calc.
        // Result is in rads and NOT in degrees. ~ rads as in radiation? ~ no, as in radians!
        // Vector3 angularDisplacement = torque / inertia;

        // Vector3 angularVelocity = angularDisplacement * Time.deltaTime;
        // return angularDisplacement * Time.deltaTime;

        // return torque;
        return torque / inertia;

        // print(angularDisplacement);
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
}
