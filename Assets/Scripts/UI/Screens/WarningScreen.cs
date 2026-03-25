using System;
using UnityEngine;

public class WarningScreen : MonoBehaviour
{
    public const string FUEL_LOW_KEY = "FUEL_LOW";
    public const string CANNON_LOW_KEY = "CANN_LOW";
    public const string MISSILE_LOW_KEY = "MISS_LOW";
    public const string COUNTERMEASURES_LOW_KEY = "CM_LOW";
    public const string DAMAGE_CRITICAL_KEY = "DMG_CRIT";
    public const string ENEMY_LOCK_KEY = "ENMY_LOCK";
    public const string MISSILE_LOCK_KEY = "MISS_LOCK";
    public const string DAMAGE_TAKEN_KEY = "DMG_TAKN";
    public const string ENEMY_DETECTED_KEY = "ENMY_DETC";
    public const string NO_LOCK_KEY = "NO_LOCK";
    public const string LOCK_FAILED_KEY = "LOCK_FAIL";

    [SerializeField]
    WarnIndicator[] warnIndicators;

    [SerializeField]
    RadarPing radarPing;

    void Awake()
    {
        radarPing.OnPinged += OnEnemyLockedUs;
    }

    public void ShowWarning(string key)
    {
        int index = GetWarnIndicatorIndexByKey(key);

        if (index == -1) return;

        warnIndicators[index].Show();
    }

    public void HideWarning(string key)
    {
        int index = GetWarnIndicatorIndexByKey(key);

        if (index == -1) return;

        warnIndicators[index].Hide();
    }

    [Obsolete("Use Hide and Show Warning for more control.")]
    public void StopFlashWarning(string key)
    {
        int index = GetWarnIndicatorIndexByKey(key);

        if (index == -1) return;

        warnIndicators[index].StopFlash();
    }

    public void FlashWarning(string key)
    {
        int index = GetWarnIndicatorIndexByKey(key);

        if (index == -1) return;

        warnIndicators[index].Flash();
    }

    public void FlashForTimeWarning(string key, float duration, bool endOn = false)
    {
        int index = GetWarnIndicatorIndexByKey(key);

        if (index == -1) return;

        warnIndicators[index].FlashForTime(duration, endOn);
    }

    private int GetWarnIndicatorIndexByKey(string key)
    {
        for (int i = 0; i < warnIndicators.Length; i++)
        {
            if (warnIndicators[i].warnKey == key) return i;
        }

        return -1;
    }

    public void OnEnemyLockedUs(Collider collider)
    {
        if (collider.gameObject.CompareTag(Constants.SHIP_TAG))
            FlashForTimeWarning(ENEMY_LOCK_KEY, 1f);
        else if (collider.gameObject.CompareTag(Constants.MISSILE_TAG))
            FlashForTimeWarning(MISSILE_LOCK_KEY, 1f);
    }
}
