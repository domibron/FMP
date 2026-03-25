using System;
using TMPro;
using UnityEngine;

public class FlightInfoScreen : MonoBehaviour
{
    [SerializeField]
    FuelTank fuelTank;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    WarningScreen warningScreen;

    [SerializeField]
    CountermeasureController countermeasureController;

    [SerializeField]
    TMP_Text uiText;

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        string displayText = "";

        FuelTankData fuelTankData = JsonUtility.FromJson<FuelTankData>(fuelTank.ReadData());
        CountermeasureControllerData cmData = JsonUtility.FromJson<CountermeasureControllerData>(countermeasureController.ReadData());

        bool fuelTankIsNull = string.IsNullOrEmpty(fuelTank.ReadData());
        bool cmIsNull = string.IsNullOrEmpty(countermeasureController.ReadData());

        string fuelWarn = fuelTankData.FuelFillNormalized > 0 ? (fuelTankData.FuelFillNormalized <= 0.3f ? "<color=red>>>>LOW FUEL<<<</color>" : "") : "<color=red>!>OUT OF FUEL<!</color>";

        if (fuelTankData.FuelFillNormalized > 0 && fuelTankData.FuelFillNormalized <= 0.3f)
        {
            warningScreen.FlashWarning(WarningScreen.FUEL_LOW_KEY);
        }
        else if (fuelTankData.FuelFillNormalized <= 0)
        {
            warningScreen.ShowWarning(WarningScreen.FUEL_LOW_KEY);
        }
        else
        {
            warningScreen.HideWarning(WarningScreen.FUEL_LOW_KEY);
        }

        string cmWarn = "ERROR";


        if (!cmIsNull)
        {
            cmWarn = $"{cmData.GetRemaining}/{cmData.GetMax} ";
            cmWarn += cmData.GetRemainingNormalized > 0 ? (cmData.GetRemainingNormalized <= 0.3f ? "<color=red>>>>LOW CMs<<<</color>" : "") : "<color=red>!>OUT OF CMs<!</color>";

            if (cmData.GetRemainingNormalized > 0 && cmData.GetRemainingNormalized <= 0.3f)
            {
                warningScreen.FlashWarning(WarningScreen.COUNTERMEASURES_LOW_KEY);
            }
            else if (cmData.GetRemainingNormalized <= 0)
            {
                warningScreen.ShowWarning(WarningScreen.COUNTERMEASURES_LOW_KEY);
            }
            else
            {
                warningScreen.HideWarning(WarningScreen.COUNTERMEASURES_LOW_KEY);
            }
        }

        displayText += $"FUEL:\n[{ArmsScreen.GetFill(ArmsScreen.FILL_BAR_AMOUNT, fuelTankData.FuelFillNormalized)}]\n {fuelTankData.FuelFillNormalized.ToString("P0")} {fuelWarn}\n";
        displayText += $"\nSPEED:\n {rb.linearVelocity.magnitude.ToString("n0")}m/s\n";
        displayText += $"\nCOUNTER MEASURES:\n {cmWarn}";


        uiText.text = displayText;
    }
}
