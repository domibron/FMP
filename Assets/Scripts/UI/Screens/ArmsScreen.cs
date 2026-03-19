using System;
using TMPro;
using UnityEngine;

public class ArmsScreen : MonoBehaviour
{
    [SerializeField] 
    private WeaponManager weaponManager;

    [SerializeField]
    TMP_Text screenText;

    private WeaponData cannonData;
    private WeaponData missileData;
    
    private void Update()
    {
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        if (string.IsNullOrEmpty(weaponManager.ReadData()))
        {
            screenText.text = "<color=red>>>WEAPONS SYSTEM FAILURE<<<";
        }

        WeaponManagerData weaponManagerData = JsonUtility.FromJson<WeaponManagerData>(weaponManager.ReadData());

        foreach (var weaponData in weaponManagerData.weaponsData)
        {
            switch (weaponData.WeaponType)
            {
                case WeaponType.Cannon:
                    cannonData = weaponData;
                    break;
                case WeaponType.Missile:
                    missileData = weaponData;
                    break;
            }
        }
        
        float cannonPercent = ((float)cannonData.WeaponAmmo / (float)cannonData.WeaponAmmoMax);
        int cannonPercentAsInt = Mathf.RoundToInt(cannonPercent * 100f);
        string cannonPercentDis = (cannonPercentAsInt < 100 ? " " : "") + (cannonPercentAsInt < 10 ? " " : "") + cannonPercentAsInt.ToString();
        string ammoPercentWarn = cannonPercent <= 0.3333f ? "<color=red>>>>LOW AMMO<<<</color>" : "";
        
        string display = $"CANNON:\n[{GetFill(18, cannonPercent)}]\n{cannonPercentDis}% {ammoPercentWarn}\n\nMISSILES:\n[]";
        
        screenText.text = display;
    }

    private string GetFill(int maxBars, float currentFill, char fillChar = '=', char spaceChar = ' ')
    {
        currentFill = Mathf.Clamp01(currentFill); // make sure no funny bugs happen.
        int barsNeeded = Mathf.RoundToInt(maxBars * currentFill);
        
        int spacersMeeded = maxBars - barsNeeded;

        string retunedFill = "";
        
        for (int i = 1; i <= barsNeeded; i++)
        {
            retunedFill += fillChar;
        }
        
        for (int i = 1; i <= spacersMeeded; i++)
        {
            retunedFill += spaceChar;
        }
        
        return retunedFill;
    }
}
