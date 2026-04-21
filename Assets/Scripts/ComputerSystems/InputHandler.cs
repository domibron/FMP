using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    InputIntermediate inputIntermediate;

    InputAction moveAction;
    InputAction lookAction;
    InputAction fireAction;
    InputAction lockOnAction;
    InputAction switchAction;
    InputAction counterMAction;

    Vector3 lookVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move", true);
        lookAction = InputSystem.actions.FindAction("Look", true);
        fireAction = InputSystem.actions.FindAction("Fire", true);
        lockOnAction = InputSystem.actions.FindAction("Lock", true);
        switchAction = InputSystem.actions.FindAction("Switch", true);
        counterMAction = InputSystem.actions.FindAction("Countermeasure", true);


        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            InputSystem.actions.LoadBindingOverridesFromJson(rebinds);
        // print(moveAction.ReadValue<Vector3>());
    }

    // Update is called once per frame
    void Update()
    {
        inputIntermediate.SetMoveVector(moveAction.ReadValue<Vector3>());

        lookVector = lookAction.ReadValue<Vector3>() * PlayerPrefs.GetFloat("Sens", 1f);
        if (PlayerPrefs.GetInt("InvertY", 0) == 1) lookVector.y = -lookVector.y;
        if (PlayerPrefs.GetInt("InvertX", 0) == 1) lookVector.x = -lookVector.x;
        inputIntermediate.SetLookVector(lookVector);

        inputIntermediate.SetFirePressed(fireAction.IsPressed());
        inputIntermediate.SetLockPressed(lockOnAction.IsPressed());
        inputIntermediate.SetSwitchPressed(switchAction.IsPressed());
        inputIntermediate.SetCounterMPressed(counterMAction.IsPressed());
    }

    // public Vector3 GetMoveInputVector()
    // {
    //     // print(moveAction.ReadValue<Vector3>());
    //     return moveAction.ReadValue<Vector3>();
    // }

    // public Vector3 GetLookInputVector()
    // {
    //     return lookAction.ReadValue<Vector3>();
    // }

    // public bool GetFirePressed()
    // {
    //     return fireAction.IsPressed();
    // }

    // public bool GetLockPressed()
    // {
    //     return lockOnAction.IsPressed();
    // }

    // public bool GetSwitchPressed()
    // {
    //     return switchAction.IsPressed();
    // }

    // public bool GetCounterMPressed()
    // {
    //     return counterMAction.IsPressed();
    // }

}
