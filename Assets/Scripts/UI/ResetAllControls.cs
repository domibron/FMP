using UnityEngine;

public class ResetAllControls : MonoBehaviour
{
    public void OnClicked()
    {
        Rebinder.Instance.ResetAll();
    }
}
