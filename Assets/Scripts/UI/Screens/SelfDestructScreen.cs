using TMPro;
using UnityEngine;

public class SelfDestructScreen : MonoBehaviour
{
    [SerializeField]
    string top = "ATTENTION!\n";

    [SerializeField]
    string selfDestructWarn = "\nINITIATING AUTOMATIC SELF DESTRUCT IN:\n";

    public string Reason = "DAMAGE IS CRITICAL!";

    public string TimerText = "T-";

    public float TimeRemainingSecs = 10;

    [SerializeField]
    TMP_Text WarnLabel;

    // Update is called once per frame
    void Update()
    {
        WarnLabel.text = top + Reason + selfDestructWarn + TimerText + TimeRemainingSecs.ToString("f0") + " SECONDS";
    }


}
