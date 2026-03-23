using UnityEngine;

public class InputIntermediate : MonoBehaviour
{
    Vector3 moveVector = Vector3.zero;
    Vector3 lookVector = Vector3.zero;

    bool firePressed = false;
    bool lockOnPressed = false;
    bool switchPressed = false;
    bool counterMPressed = false;

    public void SetMoveVector(Vector3 vector3)
    {
        moveVector = vector3;
    }

    public Vector3 GetMoveVector()
    {
        return moveVector;
    }

    public void SetLookVector(Vector3 vector3)
    {
        lookVector = vector3;
    }

    public Vector3 GetLookVector()
    {
        return lookVector;
    }

    public void SetFirePressed(bool b)
    {
        firePressed = b;
    }

    public bool GetFirePressed()
    {
        return firePressed;
    }

    public void SetLockPressed(bool b)
    {
        lockOnPressed = b;
    }

    public bool GetLockPressed()
    {
        return lockOnPressed;
    }

    public void SetSwitchPressed(bool b)
    {
        switchPressed = b;
    }

    public bool GetSwitchPressed()
    {
        return switchPressed;
    }

    public void SetCounterMPressed(bool b)
    {
        counterMPressed = b;
    }

    public bool GetCounterMPressed()
    {
        return counterMPressed;
    }
}
