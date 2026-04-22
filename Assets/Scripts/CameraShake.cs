using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    Vector3 maxBounds = new Vector3(0.5f, 0.5f, 0.5f);

    [SerializeField]
    Vector3 maxRollDeg = new Vector3(5f, 5f, 5f);

    private Vector3 currentCamSetPos = Vector3.zero;

    private Vector3 currentCamShakePos = Vector3.zero;

    private Vector3 currentCamRoll = Vector3.zero;

    float currentIntensity = 0;

    float shakeTime = 0;

    float intensityDecayRate = 1f;

    float maxShakeMove = 0.01f;

    [SerializeField]
    Rigidbody rb;

    Vector3 lastLV = Vector3.zero;

    Vector3 lastAV = Vector3.zero;

    void Update()
    {
        transform.localPosition = currentCamSetPos + currentCamShakePos;
        transform.localRotation = Quaternion.Euler(currentCamRoll);

        if (currentIntensity > 3f) currentIntensity -= (currentIntensity / 0.5f) * Time.deltaTime * intensityDecayRate;
        else if (currentIntensity > 0) currentIntensity = 0f;

        // currentIntensity += Time.deltaTime;

        if (currentIntensity <= 0)
        {
            shakeTime = 0;
            // ShakeCam(15f);
            // currentIntensity = 10f; // * DEBUG
        }
        else if (currentIntensity > 0)
        {
            shakeTime += Time.deltaTime;
        }

        HandleShakeCamera();
        HandleLinierOffset();
        HandleAngularOffset();
    }

    public void ShakeCam(float intensity)
    {
        currentIntensity += intensity;
        if (currentIntensity > 15f) currentIntensity = 15;
    }

    private void HandleShakeCamera()
    {
        float amplitude = Mathf.Lerp(0, maxShakeMove, currentIntensity / 5f);
        currentCamShakePos = new Vector3(Mathf.Sin(shakeTime * 5f * currentIntensity) * amplitude, Mathf.Sin(shakeTime * 7f * currentIntensity) * amplitude, 0);
    }

    private void HandleLinierOffset()
    {
        Vector3 localVel = rb.transform.InverseTransformDirection(rb.linearVelocity);
        Vector3 dif = localVel - lastLV;

        currentCamSetPos = Vector3.Lerp(currentCamSetPos, -dif.normalized * Mathf.Lerp(0, 0.4f, dif.magnitude / 5f), 1f * Time.deltaTime);

        lastLV = localVel;
    }

    private void HandleAngularOffset()
    {
        Vector3 localVel = rb.transform.InverseTransformDirection(rb.angularVelocity * Mathf.Rad2Deg);

        Vector3 dif = localVel - lastAV;

        currentCamRoll = Quaternion.Lerp(Quaternion.Euler(currentCamRoll), Quaternion.Euler(-dif.normalized * Mathf.Lerp(0, 5f, dif.magnitude / 5f)), 1f * Time.deltaTime).eulerAngles;

        lastAV = localVel;
    }

    public void SetOffset(Vector3 offset)
    {
        currentCamSetPos = offset;
    }
}
