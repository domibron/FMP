using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TrackingScreen : MonoBehaviour
{
    [SerializeField]
    TMP_Text uiText;

    [SerializeField]
    TrackingSystem trackingSystem;

    [SerializeField]
    WarningScreen warningScreen;

    List<Collider> missiles = new List<Collider>();
    List<Collider> ships = new List<Collider>();

    List<Collider> allEntities = new List<Collider>();

    bool lastLock = false;

    void Update()
    {
        UpdateTextDisplay();
    }

    private void UpdateTextDisplay()
    {
        string displayedText;

        if (string.IsNullOrEmpty(trackingSystem.ReadData()))
        {
            uiText.text = "<color=red>>>>CONNECTION LOST<<<</color>";
            return;
        }



        TrackingData data = JsonUtility.FromJson<TrackingData>(trackingSystem.ReadData());

        allEntities.Clear();
        allEntities.AddRange(data.storedDetectedRadarTargets.ToList());

        foreach (var target in data.storedDetectedGeneralTargets)
        {
            if (allEntities.Contains(target)) continue;

            allEntities.Add(target);
        }


        ships.Clear();
        missiles.Clear();

        // for loop here, go through all targets and check if they are missiles.
        foreach (var target in allEntities)
        {
            if (!target) continue; // if null, ignore. or well if non-existing.

            if (target.gameObject.CompareTag(Constants.SHIP_TAG))
            {
                ships.Add(target);
            }
            else if (target.gameObject.CompareTag(Constants.MISSILE_TAG))
            {
                missiles.Add(target);
            }
        }

        displayedText = $"DETECTED TARGETS:\n{ships.Count} SHIPS\n{missiles.Count} MISSILES\n";

        if (ships.Count > 0)
        {
            warningScreen.FlashWarning(WarningScreen.ENEMY_DETECTED_KEY);
        }
        else
        {
            warningScreen.HideWarning(WarningScreen.ENEMY_DETECTED_KEY);
        }

        if (!data.hasLock)
        {
            displayedText += $"\n<b>[NO LOCK]</b>";
            if (ships.Count > 0)
            {
                warningScreen.FlashWarning(WarningScreen.NO_LOCK_KEY);
            }
            else
            {
                warningScreen.HideWarning(WarningScreen.NO_LOCK_KEY);
            }

            if (lastLock)
            {
                warningScreen.FlashForTimeWarning(WarningScreen.LOCK_FAILED_KEY, 3f);
            }
        }
        else
        {
            warningScreen.HideWarning(WarningScreen.NO_LOCK_KEY);

            float speed = 0;

            if (data.lockedTarget.transform.GetComponentInParent<Rigidbody>())
            {
                speed = data.lockedTarget.transform.GetComponentInParent<Rigidbody>().linearVelocity.magnitude;
            }

            displayedText += $"\n<b>[LOCKED]</b>\nTARGET: {data.lockedTarget.gameObject.tag.ToUpper()}\nSPEED: {speed}m/s";
        }

        lastLock = data.hasLock;


        uiText.text = displayedText;
    }
}
