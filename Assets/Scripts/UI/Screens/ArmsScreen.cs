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
    private WeaponType selectedWeapon;





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

        foreach (var weaponData in weaponManagerData.WeaponsData)
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

        selectedWeapon = weaponManagerData.WeaponsData[weaponManagerData.CurrentSelectedWeapon].WeaponType;

        float cannonPercent = ((float)cannonData.WeaponAmmo / (float)cannonData.WeaponAmmoMax);
        int cannonPercentAsInt = Mathf.RoundToInt(cannonPercent * 100f);
        string cannonPercentDis = (cannonPercentAsInt < 100 ? " " : "") + (cannonPercentAsInt < 10 ? " " : "") + cannonPercentAsInt.ToString();
        string ammoPercentWarn = cannonPercent <= 0.3333f ? "<color=red>>>>LOW AMMO<<<</color>" : "";



        string cannonDisplay = "";

        if (cannonData.WeaponAmmo == -1 || cannonData.WeaponAmmoMax == -1)
        {
            cannonDisplay = $"CANNON:\n<color=red>>>>ERROR<<<\n[CONNECTION LOST]</color>";
        }
        else
        {
            cannonDisplay = $"CANNON:\n[{GetFill(18, cannonPercent)}]\n{cannonPercentDis}% {ammoPercentWarn}";
        }


        string missileDisplay = "";

        if (missileData.WeaponAmmo == -1 || missileData.WeaponAmmoMax == -1)
        {

        }
        else
        {
            missileDisplay = $"MISSILES:\n{GetBoxFill(missileData.WeaponAmmoMax, missileData.WeaponAmmo)}";
        }

        screenText.text = $"{cannonDisplay}\n\n{missileDisplay}";
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

    private string GetBoxFill(int maxBoxes, int currentFill, char fillChar = 'M', char emptyChar = ' ')
    {
        // currentFill = Mathf.Clamp01(currentFill); // make sure no funny bugs happen.
        // int barsNeeded = Mathf.RoundToInt(maxBars * currentFill);


        string retunedFill = "";

        int spaces = maxBoxes - currentFill;

        for (int i = 1; i <= maxBoxes; i++)
        {

            retunedFill += '[';

            if (i > maxBoxes - spaces) retunedFill += emptyChar;
            else retunedFill += fillChar;

            retunedFill += ']';
        }

        return retunedFill;
    }
}
