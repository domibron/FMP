using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rebinder : MonoBehaviour
{
    public static Rebinder Instance { private set; get; }

    public event Action OnRebindComplete;
    public event Action OnResetAll;

    // [SerializeField]
    // InputActionAsset inputActions;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    InputAction moveAction;
    InputAction lookAction;
    InputAction fireAction;
    InputAction lockOnAction;
    InputAction switchAction;
    InputAction counterMAction;
    InputAction dampenersAction;

    public enum InputType
    {
        PCForward,
        PCLeft,
        PCRight,
        PCBackwards,
        PCUp,
        PCDown,

        PCRollClock,
        PCRollCounterClock,
        PCLookUp,
        PCLookDown,
        PCLookLeft,
        PCLookRight,

        PCFire,
        PCSwitch,
        PCLock,
        PCCounter,
        PCDampeners,

        CONTROLLERForward,
        CONTROLLERLeft,
        CONTROLLERRight,
        CONTROLLERBackwards,
        CONTROLLERUp,
        CONTROLLERDown,

        CONTROLLERRollClock,
        CONTROLLERRollCounterClock,
        CONTROLLERLookUp,
        CONTROLLERLookDown,
        CONTROLLERLookLeft,
        CONTROLLERLookRight,

        CONTROLLERFire,
        CONTROLLERSwitch,
        CONTROLLERLock,
        CONTROLLERCounter,
        CONTROLLERDampeners,
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        moveAction = InputSystem.actions.FindAction("Move", true);
        lookAction = InputSystem.actions.FindAction("Look", true);
        fireAction = InputSystem.actions.FindAction("Fire", true);
        lockOnAction = InputSystem.actions.FindAction("Lock", true);
        switchAction = InputSystem.actions.FindAction("Switch", true);
        counterMAction = InputSystem.actions.FindAction("Countermeasure", true);
        dampenersAction = InputSystem.actions.FindAction("Dampeners", true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartRebind(InputType inputType)
    {
        print("Rebind start");

        InputSystem.actions.FindActionMap("Player").Disable();


        //.WithControlsExcluding("<keyboard>/anyKey")
        //.WithControlsExcluding("Gamepad")
        switch (inputType)
        {
            case InputType.PCForward:
                rebindingOperation = moveAction.PerformInteractiveRebinding(5).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCBackwards:
                rebindingOperation = moveAction.PerformInteractiveRebinding(6).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCLeft:
                rebindingOperation = moveAction.PerformInteractiveRebinding(3).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCRight:
                rebindingOperation = moveAction.PerformInteractiveRebinding(4).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCUp:
                rebindingOperation = moveAction.PerformInteractiveRebinding(1).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCDown:
                rebindingOperation = moveAction.PerformInteractiveRebinding(2).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;

            case InputType.PCRollClock:
                rebindingOperation = lookAction.PerformInteractiveRebinding(5).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCRollCounterClock:
                rebindingOperation = lookAction.PerformInteractiveRebinding(6).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCLookUp:
                rebindingOperation = lookAction.PerformInteractiveRebinding(1).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCLookDown:
                rebindingOperation = lookAction.PerformInteractiveRebinding(2).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCLookLeft:
                rebindingOperation = lookAction.PerformInteractiveRebinding(3).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCLookRight:
                rebindingOperation = lookAction.PerformInteractiveRebinding(4).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;

            case InputType.PCFire:
                rebindingOperation = fireAction.PerformInteractiveRebinding(0).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCSwitch:
                rebindingOperation = switchAction.PerformInteractiveRebinding(0).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCLock:
                rebindingOperation = lockOnAction.PerformInteractiveRebinding(0).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCCounter:
                rebindingOperation = counterMAction.PerformInteractiveRebinding(0).WithControlsExcluding("Gamepad").WithCancelingThrough("<keyboard>/Escape").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.PCDampeners:
                rebindingOperation = dampenersAction.PerformInteractiveRebinding(0).WithControlsExcluding("Gamepad").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;

            case InputType.CONTROLLERForward:
                rebindingOperation = moveAction.PerformInteractiveRebinding(12).WithControlsExcluding("<keyboard>/anyKey").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERBackwards:
                rebindingOperation = moveAction.PerformInteractiveRebinding(13).WithControlsExcluding("<keyboard>/anyKey").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERLeft:
                rebindingOperation = moveAction.PerformInteractiveRebinding(10).WithControlsExcluding("<keyboard>/anyKey").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERRight:
                rebindingOperation = moveAction.PerformInteractiveRebinding(11).WithControlsExcluding("<keyboard>/anyKey").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERUp:
                rebindingOperation = moveAction.PerformInteractiveRebinding(8).WithControlsExcluding("<keyboard>/anyKey").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERDown:
                rebindingOperation = moveAction.PerformInteractiveRebinding(9).WithControlsExcluding("<keyboard>/anyKey").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;

            case InputType.CONTROLLERRollClock:
                rebindingOperation = lookAction.PerformInteractiveRebinding(12).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERRollCounterClock:
                rebindingOperation = lookAction.PerformInteractiveRebinding(13).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERLookUp:
                rebindingOperation = lookAction.PerformInteractiveRebinding(8).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERLookDown:
                rebindingOperation = lookAction.PerformInteractiveRebinding(9).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERLookLeft:
                rebindingOperation = lookAction.PerformInteractiveRebinding(10).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERLookRight:
                rebindingOperation = lookAction.PerformInteractiveRebinding(11).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;

            case InputType.CONTROLLERFire:
                rebindingOperation = fireAction.PerformInteractiveRebinding(1).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERSwitch:
                rebindingOperation = switchAction.PerformInteractiveRebinding(1).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERLock:
                rebindingOperation = lockOnAction.PerformInteractiveRebinding(1).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERCounter:
                rebindingOperation = counterMAction.PerformInteractiveRebinding(1).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
            case InputType.CONTROLLERDampeners:
                rebindingOperation = dampenersAction.PerformInteractiveRebinding(1).WithControlsExcluding("<keyboard>/anyKey").WithControlsExcluding("Mouse").OnComplete(operation => RebindCompleted()).OnCancel(op => RebindCompleted());
                break;
        }
        // rebindingOperation = moveAction.PerformInteractiveRebinding().OnComplete(operation => RebindCompleted(inputType));
        rebindingOperation.Start();
    }

    private void RebindCompleted()
    {
        rebindingOperation.Dispose();

        // string newBinding = moveAction.bindings[0].effectivePath;
        // print(newBinding);

        InputSystem.actions.FindActionMap("Player").Enable();

        var rebinds = InputSystem.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);

        OnRebindComplete?.Invoke();

        print("Rebind end");
    }

    public string GetInput(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.PCForward:
                return moveAction.GetBindingDisplayString(5);
            case InputType.PCBackwards:
                return moveAction.GetBindingDisplayString(6);
            case InputType.PCLeft:
                return moveAction.GetBindingDisplayString(3);
            case InputType.PCRight:
                return moveAction.GetBindingDisplayString(4);
            case InputType.PCUp:
                return moveAction.GetBindingDisplayString(1);
            case InputType.PCDown:
                return moveAction.GetBindingDisplayString(2);

            case InputType.PCRollClock:
                return lookAction.GetBindingDisplayString(5);
            case InputType.PCRollCounterClock:
                return lookAction.GetBindingDisplayString(6);
            case InputType.PCLookUp:
                return lookAction.GetBindingDisplayString(1);
            case InputType.PCLookDown:
                return lookAction.GetBindingDisplayString(2);
            case InputType.PCLookLeft:
                return lookAction.GetBindingDisplayString(3);
            case InputType.PCLookRight:
                return lookAction.GetBindingDisplayString(4);

            case InputType.PCFire:
                return fireAction.GetBindingDisplayString(0);
            case InputType.PCSwitch:
                return switchAction.GetBindingDisplayString(0);
            case InputType.PCLock:
                return lockOnAction.GetBindingDisplayString(0);
            case InputType.PCCounter:
                return counterMAction.GetBindingDisplayString(0);
            case InputType.PCDampeners:
                return dampenersAction.GetBindingDisplayString(0);

            case InputType.CONTROLLERForward:
                return moveAction.GetBindingDisplayString(12);
            case InputType.CONTROLLERBackwards:
                return moveAction.GetBindingDisplayString(13);
            case InputType.CONTROLLERLeft:
                return moveAction.GetBindingDisplayString(10);
            case InputType.CONTROLLERRight:
                return moveAction.GetBindingDisplayString(11);
            case InputType.CONTROLLERUp:
                return moveAction.GetBindingDisplayString(8);
            case InputType.CONTROLLERDown:
                return moveAction.GetBindingDisplayString(9);

            case InputType.CONTROLLERRollClock:
                return lookAction.GetBindingDisplayString(12);
            case InputType.CONTROLLERRollCounterClock:
                return lookAction.GetBindingDisplayString(13);
            case InputType.CONTROLLERLookUp:
                return lookAction.GetBindingDisplayString(8);
            case InputType.CONTROLLERLookDown:
                return lookAction.GetBindingDisplayString(9);
            case InputType.CONTROLLERLookLeft:
                return lookAction.GetBindingDisplayString(10);
            case InputType.CONTROLLERLookRight:
                return lookAction.GetBindingDisplayString(11);

            case InputType.CONTROLLERFire:
                return fireAction.GetBindingDisplayString(1);
            case InputType.CONTROLLERSwitch:
                return switchAction.GetBindingDisplayString(1);
            case InputType.CONTROLLERLock:
                return lockOnAction.GetBindingDisplayString(1);
            case InputType.CONTROLLERCounter:
                return counterMAction.GetBindingDisplayString(1);
            case InputType.CONTROLLERDampeners:
                return dampenersAction.GetBindingDisplayString(1);
        }

        return "FAILED";
    }

    public void ResetAll()
    {
        InputSystem.actions.RemoveAllBindingOverrides();
        PlayerPrefs.SetString("rebinds", string.Empty);
        OnResetAll?.Invoke();
    }

    public bool IsCurrentlyRebinding()
    {
        if (rebindingOperation != null)
            return true;

        return false;
    }

    public void MakeSureToStopAllRebinds()
    {
        rebindingOperation.Cancel();
    }
}
