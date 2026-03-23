using UnityEngine;

public class InputPasser : MonoBehaviour
{
    [SerializeField]
    TrackingSystem trackingSystem;

    InputIntermediate inputIntermediate;

    bool lockPressed = false;

    void Awake()
    {
        inputIntermediate = GetComponent<InputIntermediate>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inputIntermediate.GetLockPressed() && !lockPressed)
        {
            lockPressed = true;
            trackingSystem.TryLockTargetNearCenter();
        }
        else
        {
            lockPressed = false;
        }
    }
}
