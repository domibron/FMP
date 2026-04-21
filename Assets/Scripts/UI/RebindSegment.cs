using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindSegment : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    TMP_Text label;

    [SerializeField]
    Rebinder.InputType action;

    void Start()
    {
        label.text = Rebinder.Instance.GetInput(action);
    }

    void OnEnable()
    {
        Rebinder.Instance.OnRebindComplete += OnRebindCompleted;
        Rebinder.Instance.OnResetAll += OnRebindCompleted;
    }


    void OnDisable()
    {
        Rebinder.Instance.OnRebindComplete -= OnRebindCompleted;
        Rebinder.Instance.OnResetAll -= OnRebindCompleted;
    }

    public void OnClick()
    {
        if (!Rebinder.Instance) return;

        if (Rebinder.Instance.IsCurrentlyRebinding())
        {
            Rebinder.Instance.MakeSureToStopAllRebinds();
        }

        Rebinder.Instance.StartRebind(action);
        button.interactable = false;
        label.text = "REBINDING";
    }

    void OnRebindCompleted()
    {
        button.interactable = true;
        label.text = Rebinder.Instance.GetInput(action);
    }
}
