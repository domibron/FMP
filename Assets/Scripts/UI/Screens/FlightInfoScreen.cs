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
        string fuelWarn = fuelTankData.FuelFillNormalized > 0 ? (fuelTankData.FuelFillNormalized <= 0.3f ? "<color=red>>>>LOW FUEL<<<</color>" : "") : "<color=red>!>OUT OF FUEL<!</color>";
        string cmWarn = cmData.GetRemainingNormalized > 0 ? (cmData.GetRemainingNormalized <= 0.3f ? "<color=red>>>>LOW CMs<<<</color>" : "") : "<color=red>!>OUT OF CMs<!</color>";

        displayText += $"FUEL:\n[{ArmsScreen.GetFill(ArmsScreen.FILL_BAR_AMOUNT, fuelTankData.FuelFillNormalized)}]\n {fuelTankData.FuelFillNormalized.ToString("P0")} {fuelWarn}\n";
        displayText += $"\nSPEED:\n {rb.linearVelocity.magnitude.ToString("n0")}m/s\n";
        displayText += $"\nCOUNTER MEASURES:\n {cmData.GetRemaining}/{cmData.GetMax} {cmWarn}";


        uiText.text = displayText;
    }
}
