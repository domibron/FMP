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

    float maxAngle = 45;

    [SerializeField]
    Transform thrusterY;

    [SerializeField]
    Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get the needed rotational angle.

        // plug that in.


        // Vector 3 rotation is in world space. we need the diff in local.
        // Vector3 displacementFromTarget = Vector3.Cross((target.position - rb.transform.position).normalized, rb.transform.forward).normalized;
        // displacementFromTarget = displacementFromTarget * Vector3.Angle((target.position - rb.transform.position).normalized, rb.transform.forward.normalized);
        // print("Dis " + displacementFromTarget);

        // Vector2 displacement2D = new Vector2(-displacementFromTarget.y, displacementFromTarget.x);

        // displacement2D /= maxAngle;



        // XDot *= 90f;
        // YDot *= 90f;

        // XDot /= maxAngle;
        // YDot /= maxAngle;

        // print("DOTS: " + (Mathf.Asin(XDot) * Mathf.Rad2Deg) / maxAngle + ", " + (Mathf.Asin(YDot) * Mathf.Rad2Deg) / maxAngle);

        // Vector3 forward = rb.transform.InverseTransformDirection(rb.transform.forward);

        // Debug.DrawLine(rb.transform.position, rb.transform.position + rb.transform.TransformDirection(Vector3.Cross(forward, directionToTarget)));

        // float angle = Vector3.Angle(directionToTarget, forward);
        // // print("asdasda " + directionToTarget);

        // float mag = directionToTarget.magnitude;
        // directionToTarget.z = 0;
        // directionToTarget.Normalize();

        // take target and use max angle as direction to target is in 180 sections.

        // print("dir to targ " + directionToTarget);

        Vector3 forward = rb.transform.forward.normalized;
        Vector3 targetDir = (target.position - rb.transform.position).normalized;

        Vector3 angleAxis = Vector3.Cross(forward, targetDir);
        angleAxis = rb.transform.InverseTransformDirection(angleAxis);


        float angle = (Mathf.Acos(Vector3.Dot(forward, targetDir))) / (targetDir.magnitude * forward.magnitude);

        Vector3 rotationAngle = angleAxis.normalized * angle;// in rads, also angular vel is in rads too.

        Debug.DrawLine(rb.transform.position, rb.transform.position + angleAxis.normalized, Color.blue);

        // print("ROT " + rotationAngle + " cur anvel " + rb.transform.InverseTransformDirection(rb.angularVelocity));

        Vector3 localAngularVel = rb.transform.InverseTransformDirection(rb.angularVelocity);

        float AngleAgressive = (angle * Mathf.Rad2Deg) / maxAngle;
        Vector3 diff = (rotationAngle * 2f).normalized - localAngularVel.normalized;

        Vector2 input = new Vector3(diff.y, -diff.x).normalized * AngleAgressive;

        AngleThrusterToTarget(input);

        print(input + " - " + diff + "  , " + rotationAngle.normalized + " . " + localAngularVel.normalized);

        // Quaternion CurrentRot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        // Quaternion TargetRot = Quaternion.FromToRotation(forward, targetDir) * CurrentRot;

        // AngleThrusterToTarget(GetInputVector(target.position));

        // AngleThrusterToTarget(new Vector2(x, y));

        // AngleThrusterToTarget(displacement2D);

        Vector3 rot = GetTorque(thruster);
        // print(rot + " ? " + Vector3.Angle(thruster.transform.forward, rb.transform.forward));
    }

    private Vector2 GetInputVector(Vector3 targetPosWorld)
    {
        const float MAX_TURN_RATE = 30f;

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
        // Res is in rads and NOT in degrees. ~ rads as in radiation? ~ no, as in radians!
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
