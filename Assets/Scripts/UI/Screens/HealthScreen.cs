using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class HealthScreenCompCollection
{
    public ComponentBase[] ComponentBases;

    public string NameIfMany = "Name";

    public string Name
    {
        get
        {
            if (ComponentBases.Length == 1)
            {
                return ComponentBases[0].ComponentName.ToUpper();
            }
            else
            {
                return NameIfMany.ToUpper();
            }
        }
    }

    public float AvgHealthNormalized
    {
        get
        {
            float avg = 0;

            foreach (var component in ComponentBases)
            {
                avg += component.HealthNormalized;
            }

            avg /= ComponentBases.Length;

            return avg;
        }
    }
}

public class HealthScreen : MonoBehaviour
{
    [SerializeField]
    TMP_Text uiText;

    [SerializeField]
    WarningScreen warningScreen;

    [SerializeField]
    HealthScreenCompCollection[] allComps;

    float lastAverage = 1f;

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        string displayText = "SHIP HEALTH:\n";

        float collectiveAvg = 0;

        string modulesText = "";

        foreach (var comp in allComps)
        {
            modulesText += $"\n{FitToBounds(comp.Name + ":")} {WrapWithColor(comp.AvgHealthNormalized, (comp.AvgHealthNormalized).ToString("P0"))}";
            collectiveAvg += comp.AvgHealthNormalized;
        }

        collectiveAvg /= allComps.Length;

        displayText += $"<b>OVERALL:</b> {WrapWithColor(collectiveAvg, (collectiveAvg).ToString("P0"))}{modulesText}";

        if (collectiveAvg > 0 && collectiveAvg <= 0.65f)
        {
            warningScreen.FlashWarning(WarningScreen.DAMAGE_CRITICAL_KEY);
        }
        else if (collectiveAvg <= 0)
        {
            warningScreen.ShowWarning(WarningScreen.DAMAGE_CRITICAL_KEY);
        }
        else
        {
            warningScreen.HideWarning(WarningScreen.DAMAGE_CRITICAL_KEY);
        }

        if (collectiveAvg < lastAverage)
        {
            warningScreen.FlashForTimeWarning(WarningScreen.DAMAGE_TAKEN_KEY, 2);
        }

        lastAverage = collectiveAvg;


        uiText.text = displayText;
    }

    private string WrapWithColor(float value, string text)
    {
        if (value >= 1)
        {
            return $"<color=white>{text}</color>";
        }
        else if (value > .6)
        {
            return $"<color=yellow>{text}</color>";
        }
        else if (value > .2)
        {
            return $"<color=orange>{text}</color>";
        }
        else if (value > 0)
        {
            return $"<color=red>{text}</color>";
        }
        else
        {
            return $"<color=#990000>{text}</color>";
        }
    }

    private string FitToBounds(string text, int max = 14)
    {
        if (text.Length > max)
        {
            return text.Remove(max + 1);
        }
        else if (text.Length < max)
        {
            int diff = max - text.Length;

            for (int i = 0; i < diff; i++)
            {
                text += " ";
            }

            return text;
        }
        else
        {
            return text;
        }
    }
}
