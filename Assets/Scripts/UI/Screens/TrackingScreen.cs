using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackingScreen : MonoBehaviour
{
    [SerializeField]
    TMP_Text uiText;

    [SerializeField]
    TrackingSystem trackingSystem;

    List<Collider> missiles;
    List<Collider> ships;

    List<Collider> allEntities;

    private void UpdateTextDisplay()
    {
        string displayedText;

        if (string.IsNullOrEmpty(trackingSystem.ReadData()))
        {
            uiText.text = "<color=red>>>>CONNECTION LOST<<<</color>";
            return;
        }

        TrackingData data = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

        allEntities.AddRange(data.storedDetectedRadarTargets);

        foreach (var target in data.storedDetectedGeneralTargets)
        {
            if (allEntities.Contains(target)) continue;

            allEntities.Add(target);
        }

        // for loop here, go through all targets and check if they are missiles.

    }
}
