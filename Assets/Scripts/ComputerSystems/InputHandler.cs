using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    InputAction moveAction;
    InputAction lookAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move", true);
        lookAction = InputSystem.actions.FindAction("Look", true);

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
}
