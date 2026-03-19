using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    InputAction moveAction;
    InputAction lookAction;
    InputAction fireAction;
    InputAction lockOnAction;
    InputAction switchAction;
    InputAction counterMAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move", true);
        lookAction = InputSystem.actions.FindAction("Look", true);
        fireAction = InputSystem.actions.FindAction("Fire", true);
        lockOnAction = InputSystem.actions.FindAction("Lock", true);
        switchAction = InputSystem.actions.FindAction("Switch", true);
        counterMAction = InputSystem.actions.FindAction("Countermeasure", true);

        // print(moveAction.ReadValue<Vector3>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetMoveInputVector()
    {
        // print(moveAction.ReadValue<Vector3>());
        return moveAction.ReadValue<Vector3>();
    }

    public Vector3 GetLookInputVector()
    {
        return lookAction.ReadValue<Vector3>();
    }

    public bool GetFirePressed()
    {
        return fireAction.IsPressed();
    }

    public bool GetLockPressed()
    {
        return lockOnAction.IsPressed();
    }

    public bool GetSwitchPressed()
    {
        return switchAction.IsPressed();
    }
    
    public bool GetCounterMPressed()
    {
        return counterMAction.IsPressed();
    }
    
}
